namespace CatApp {
  class CatApp 
  {
     static void Main(string[] args) {
      CatAPIRequest catRequest = new() ; 
      bool promptSuccess = false; 
      while(!promptSuccess ) {
        promptSuccess = catRequest.Prompt(); 
      }
      catRequest.GetCat().GetAwaiter().GetResult(); 

      // this concludes the functionality for retrieving one cat. todo next week : add to favorites list which we can store in a database 

      
    }
  }
}