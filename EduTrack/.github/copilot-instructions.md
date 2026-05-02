# Copilot Instructions

## General Guidelines
- In SQL stored procedures, avoid using SELECT *; always list explicit column names.
- Use explicit column names in SQL queries to enhance clarity and maintainability.
- Provide detailed, specific error messages in authentication flows (registration and login) rather than generic messages. Error messages should explain exactly why an action failed (e.g., "Email already exists", "Account is inactive", "Email not found") to help users understand what went wrong and how to fix it.

## Project-Specific Rules
- Project: EduTrack (ASP.NET Core 10 + SQL Server).
- Implement new models: Subject, Fees, StudentFees with full services and interfaces.
- Use the soft-delete pattern and maintain audit trails (Created_By/Date, Modified_By/Date).
- All services should be registered in Program.cs with Dependency Injection (DI).
- Database deployment should be managed via Master_Deployment_Script.sql in the EduTrack.DB project.