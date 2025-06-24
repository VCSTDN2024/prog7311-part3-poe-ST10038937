# Agri-Energy Connect Platform - Final Prototype

![Version](https://img.shields.io/badge/version-2.0.0-blue.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)

> An enterprise-grade web platform connecting South African farmers with sustainable energy solutions, featuring a community forum, audit logging, and advanced accessibility.

## Table of Contents

- [Project Overview](#project-overview)
- [YouTube Video Demonstration](#youtube-video-demonstration)
- [Key Features](#key-features)
  - [Core Functionality](#core-functionality)
  - [Community & Collaboration](#community--collaboration)
  - [Enterprise & Security](#enterprise--security)
  - [User Experience & Accessibility](#user-experience--accessibility)
- [Technical Details](#technical-details)
  - [Architecture & Patterns](#architecture--patterns)
  - [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup and Run](#setup-and-run)
  - [Demo Accounts](#demo-accounts)
- [Enterprise Software System Characteristics](#enterprise-software-system-characteristics)
- [Project Structure](#project-structure)
- [Development Approach](#development-approach)
- [Troubleshooting](#troubleshooting)
- [Screenshots](#screenshots)
- [License](#license)

## Project Overview
This final prototype for the Agri-Energy Connect Platform represents a complete, enterprise-ready web application. Building upon the initial foundation, this version introduces several advanced features designed to enhance security, user engagement, and accessibility. Key additions include a fully interactive community forum, comprehensive audit logging, seamless multi-language support, and a modernized user interface, making the platform robust, scalable, and user-centric.

## Github
https://github.com/VCSTDN2024/prog7311-part3-poe-ST10038937.git

## YouTube Video Demonstration
https://youtu.be/mD866JBUwnQ

## Key Features

### Core Functionality
- **Product Management:** Full CRUD (Create, Read, Update, Delete) capabilities for farmers to manage their product listings.
- **Farmer Management:** Enables employees to manage farmer profiles and view their associated products.
- **Dashboard Analytics:** Role-specific dashboards with data visualizations (charts) and widgets providing key insights.

### Community & Collaboration
- **Community Forum:** An interactive forum for farmers and employees to ask questions, share knowledge, and engage in discussions.
- **Posts & Replies:** Users can create new posts and reply to existing ones, fostering a collaborative environment.

### Enterprise & Security
- **Authentication & Authorization:** Secure login with ASP.NET Core Identity, enforcing strict role-based access control (Farmer vs. Employee).
- **Audit Logging:** A comprehensive audit trail system that logs critical user actions (e.g., user login, product creation) for security and compliance.
- **User Avatars:** Users can upload and manage their own profile pictures.

### User Experience & Accessibility
- **AJAX-Powered Localization:** Seamless, no-refresh language switching between English, Afrikaans, and isiZulu.
- **Modern UI/UX:** A complete visual overhaul featuring an earth-tone color palette, glassmorphism effects, and a farm-themed background.
- **Comprehensive Accessibility:** The platform meets modern accessibility standards (A11y), with semantic HTML, ARIA attributes, full keyboard navigation, and high-contrast design elements.

## Technical Details

### Architecture & Patterns
- **Model-View-Controller (MVC):** The core architectural pattern separating application logic, data, and presentation.
- **Repository Pattern:** Implemented via Entity Framework Core to abstract the data access layer.
- **Observer Pattern:** Used to create a decoupled notification system, enabling real-time updates without tight component coupling.
- **Service-Oriented Design:** Core logic, such as `IdentityRole` management and audit logging, is encapsulated in dedicated service layers for improved maintainability and testability.

### Technologies Used
- **Backend:** ASP.NET Core MVC (.NET 9.0)
- **Database:** Entity Framework Core with SQLite (easily configurable for SQL Server)
- **Authentication:** ASP.NET Core Identity
- **Frontend:** Bootstrap 5, jQuery, AJAX
- **Charting:** Chart.js for dashboard visualizations
- **Styling:** Custom CSS with modern design principles (glassmorphism, responsive design)

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio 2022, Visual Studio Code, or any compatible IDE

### Setup and Run

1.  **Clone the repository.**
2.  **Navigate to the project directory** in your terminal.
3.  Run `dotnet restore` to install all dependencies.
4.  Run `dotnet run` to build and start the application.
5.  Access the application in your browser, typically at `https://localhost:7198` or `http://localhost:5038`.

### Demo Accounts
- **Employee Account:**
  - **Email:** `employee@agrienergy.com`
  - **Password:** `Employee1!`
- **Farmer Account:**
  - **Email:** `john@farm.com`
  - **Password:** `Farmer1!`

## Enterprise Software System Characteristics

- **Security:** Enforced through role-based access, secure password hashing, and a comprehensive audit trail system.
- **Scalability:** Built on ASP.NET Core with a decoupled architecture (Observer pattern, services) to handle growth efficiently.
- **Reliability:** Ensured through robust error handling, data validation, and transactional database operations.
- **Accessibility:** A core principle of the design, ensuring the platform is usable by people with a wide range of disabilities.
- **Maintainability:** A clean, well-structured codebase with separation of concerns makes the system easy to update and extend.

## Project Structure

- **/Controllers:** Handles user requests and business logic.
- **/Data:** Contains the `DbContext` and database migrations.
- **/Models:** Contains all data models (`ApplicationUser`, `Product`, `Farmer`, `ForumPost`, `PostReply`, `AuditLog`).
- **/Services:** Contains business logic services (`IAuditService`, `IObserver`, etc.).
- **/Views:** Contains all UI-related `.cshtml` files.
- **/Resources:** Contains `.resx` files for localization.
- **/wwwroot:** Static assets (CSS, JavaScript, images, libraries).
- **/Areas/Identity:** Custom authentication and user management pages.

## Development Approach
This prototype was developed using an iterative, Agile-inspired approach. Each development cycle focused on delivering a complete, functional feature set, allowing for continuous feedback and refinement. The project evolved from a basic CRUD application to a feature-rich enterprise platform by incrementally adding and testing the Community Forum, Audit Logging, Localization, and other advanced features.

## Troubleshooting

- **Database Issues:** If you encounter database errors on startup, delete the `app.db` file in the project root and restart the application. Entity Framework will recreate it automatically.
- **Build Errors:** Run `dotnet clean` followed by `dotnet restore` to resolve potential dependency issues.
- **File Locked Errors:** Ensure no previous instances of the application are still running in the background. Use your system's Task Manager to terminate any lingering `ST10038937_prog7311_poe1.exe` processes.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---
*ST10038937_prog7311_poe1 - Agri-Energy Connect Platform © 2025*
