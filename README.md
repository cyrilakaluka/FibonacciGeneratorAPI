<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/cyrilakaluka/FibonacciGeneratorAPI">
    <img src="https://user-images.githubusercontent.com/31706741/150674293-1e6e9aea-665f-4e37-9306-f898bcfda1e7.png" alt="Logo" width="100" height="100">
  </a>

  <h2 align="center">Fibonacci Subsequence Generator API</h2>

  <p align="center">
    A web API for generating a subsequence from a sequence of Fibonacci numbers
    <br />
    <a href="https://github.com/cyrilakaluka/FibonacciGeneratorAPI"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/cyrilakaluka/FibonacciGeneratorAPI/issues">Report Bug</a>
    ·
    <a href="https://github.com/cyrilakaluka/FibonacciGeneratorAPI/issues">Request Feature</a>
  </p>
 </div> 
<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
        <li><a href="#important-links-and-documents">Importand Links and Documents</a></li>
      </ul>
    </li>
    <li><a href="#how-to-run">How to run</a></li>
    <li><a href="#swagger-documentation">Swagger Documenation</a></li>
    <li><a href="#postman-documentation">Postman Documentation</a></li>
    <li><a href="#unit-test">Unit Test</a></li>
  </ol>
</details>
  
<!-- ABOUT THE PROJECT -->
## About The Project
This project was implemented to fulfil part of the procedures for a job interview. The orignial requirements for the project were analysed to create a [User Story Specification](https://docs.google.com/document/d/1vHNFNm9OOx84OxGjZptgGecN49NFmc5xeFrbJQGE3CQ/) document. The goal of the API is to provide an endpoint for returning a subsequence from a Fibonacci sequence given a specified start and end index.
  
### Built With
* [.Net 5](https://dotnet.microsoft.com/)
* [xUnit](https://xunit.net/)
* [Swagger](https://swagger.io/)
* [Serilog](https://serilog.net/)
* [Moq 4.x](https://www.moqthis.com/moq4/)

### Important Links and Documents
User Story Specification - [View on google docs](https://docs.google.com/document/d/1vHNFNm9OOx84OxGjZptgGecN49NFmc5xeFrbJQGE3CQ)

Project Management Board - [View](https://trello.com/b/W0jGPTKd/fibonacci-generator)

Postman API documentation - [View](https://documenter.getpostman.com/view/19276579/UVXqECwb)
  
### How to run
1. Clone the repository `git clone https://github.com/cyrilakaluka/FibonacciGeneratorAPI`.
2. Build the solution using Visual Studio, or on the [command line](https://www.microsoft.com/net/core) with `dotnet build`.
3. Select the `FibonacciGeneratorAPI` profile and start the project. 
![SelectProjectProfile](https://user-images.githubusercontent.com/31706741/150678700-afc2d4a3-24cd-43be-b2e9-af0e7fe04580.png)<br/>
The API will startup by listening on [https://localhost:5001](https://localhost:5001) and [http://localhost:5000](http://localhost:5000).

### Swagger Documentation
This API supports Swagger. The Swagger UI should automatically launch on the default machine web browser after project startup. However, you can manually view the Swagger UI on a web browser by visiting [http://localhost:5000/index.html](http://localhost:5000/index.html) after project startup.

### Postman Documentation
A postman documentation is available for running integration tests on the API. See the [important documents](#important-links-and-documents) section.

### Unit Test
The API contains a unit test project. There are two ways to run the unit test:
1. Right click on the `FibonacciGeneratorAPI.UnitTests` project in Visual Studio and select `Run Tests` from the context menu.
2. Open the project solution directory in `PowerShell` and run `dotnet test` CLI command.
