using System.Net.Http.Headers; 

class CatAPIRequest {
  // tagString will be used for API request 
  private string _tagString = ""  ; 
  // tagArray may be used for storage of the characteristics in the database 
  private string[] _tagArray = []; 

  private DateTime _reqDate = DateTime.Now; 

  private string _prompt = "" ;

  private static HttpClient s_client = new() 
  {
    BaseAddress = new Uri("https://cataas.com/cat/"), 
  }; 

  public CatAPIRequest() 
  {  
    BuildPrompt() ; 
  }

  public CatAPIRequest(string tagString ) 
  {
    this.SetTags(tagString);  
    
  }

  private void BuildPrompt() {
    this._prompt += "Welcome to the cat app!\n"; 
    this._prompt += "Enter the type of cat you want to see.\n"; 
    this._prompt += "Options include: color , emotion, cuteness.\n"; 
    this._prompt += "For multiple options, separate with a comma.\n"; 
    this._prompt += "ex. input 'orange, cute' => a picture of an orange cute 🐈\n"; 
    this._prompt += "Now, enter your cat characteristics: " ;

  }

  public bool Prompt() 
  {
    Console.WriteLine(this._prompt);

    string characteristics =  Console.ReadLine() ?? "";

    if (characteristics == "") {
      Console.WriteLine("You didn't enter anything! 😭");
      Console.WriteLine("Try again. \n");
      return false ;
    } else {
      this.SetTags(characteristics); 
      return true; 
    }
  }
 
  public void SetTags(string tagString) 
  {
      this._tagString = tagString; 
      this._tagArray = tagString.Split(',');
  }

  private string FormatDateTime() {
    string dateTimeString = "" ;
    dateTimeString += this._reqDate.Year + "-";  
    dateTimeString += this._reqDate.Month + "-";
    dateTimeString += this._reqDate.Day + "_";
    dateTimeString += this._reqDate.Hour + ":";
    dateTimeString += this._reqDate.Minute + ":";
    dateTimeString += this._reqDate.Second;
   
   return dateTimeString;  
  }

  // GetCat retrieves cat with _tags from CatAPI 
  async public Task GetCat() 
  {
    
    System.Console.WriteLine($" Tags: {this._tagString} \n");
    System.Console.WriteLine($" Retreiving Cat ... ");
    // record precise time API call was made 
    this._reqDate = DateTime.Now; 

    using HttpResponseMessage response = await s_client.GetAsync(_tagString); 

    try {
      response.EnsureSuccessStatusCode();
      var streamResponse = await response.Content.ReadAsStreamAsync(); 
   
      string fout = $"./cats/cat_{String.Join("_",this._tagArray)}_{FormatDateTime()}.jpg"; 
      Stream output = File.OpenWrite(fout); 
      streamResponse.CopyTo(output); 
      Console.WriteLine("Success! Your cat photo is located at: " + fout);
    
    } catch (HttpRequestException e) {
      System.Console.WriteLine("Sorry, that didn't work! Try another type of cat 😅");
      
      
      // log exception to file 
      File.WriteAllText("./logs/log.txt",e.Message); 
    }
  



  }
}