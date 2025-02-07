# Firecrawl API Testing 
### This project used Firecrawl Extract API to extract the URL details (you can change the schema and prompt for the request body that you want to set to the POST API endpoints ) 

1) Create a free account and create an APi token in [FireCrawl](https://www.firecrawl.dev/)

2) replace the firecrawlApiKey with your actual API key, in the WebCrawl.razor
 
3) Click the buttons from the WebCrawl razor to configure the URLs and keywords (pop out modal)

4) URL must contain https:// or http://

5) The POST API will return a job ID, you can check the job id by sending GET method to endpoints - https://api.firecrawl.dev/v1/extract/{the_job_id}

# Reference: [FireCrawlDocs](https://docs.firecrawl.dev/api-reference/introduction)




