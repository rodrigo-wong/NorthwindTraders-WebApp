# Northwind Traders Web Application and API
## ASP.NET Core MVC & Web API Solution
This project is a dual-application solution simulating a fictitious company, Northwind Traders, built as part of an ASP.NET Core assignment. It consists of an MVC web app and a Web API designed to interact with a SQL Server LocalDB database.
Key Features
1. **NorthwindAPI** *(ASP.NET Core Web API)*
    - **Products Endpoint:** Returns products filtered by category and excludes discontinued items.
    - **Categories Endpoint:** Fetches all product categories.
    - **API Integration:** Configured with Swagger for API testing and documentation.
2. **Northwind Web Application** *(ASP.NET Core MVC)*
    - **Product Listing & Details:**
      - Displays products by category with a dynamic dropdown list.
      - Product details view retrieves the category name using API calls.
    - **Error Handling:** Displays user-friendly error messages for API issues.
    - **Authorization:**
      - Index: Accessible anonymously.
      - Details: Requires user authentication.
    - **Custom Views:**
      - Updated _Layout.cshtml with a modernized menu, title, and footer.
      - Customized login page for enhanced usability.
