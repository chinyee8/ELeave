# E-Leave — Employee Self-Service Leave Management System

Project Description

E-Leave is a web-based leave management system built with ASP.NET Core Web API and a VB/C# MVC frontend. It allows employees to manage their leave requests efficiently while enabling managers to track and approve requests securely.

Key Features
Employee login and signup with secure password hashing.
Submit, view, and track leave requests (annual, sick, emergency, unpaid).
Manager/approver workflow for leave approvals.
JWT-based authentication for API security.
Session management for the MVC frontend.
EF Core database integration with SQL Server, supporting User, LeaveRequest, and LeaveBalance entities.
Automatic GUID primary keys for all main entities.
Tech Stack
Backend: ASP.NET Core Web API (.NET 8)
Frontend: ASP.NET MVC, Razor Views, Bootstrap 5
Database: SQL Server with EF Core ORM
Security: BCrypt password hashing, JWT authentication
