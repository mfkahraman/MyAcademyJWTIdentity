# 🔐 JWT Auth API with ASP.NET Core

This is a simple Web API project built with **ASP.NET Core**.  
The goal of this project is to **learn how JWT (JSON Web Token) works** and how to use it in real projects.

With this API, users can **register**, **log in**, and **get a token**.  
If they are logged in and have the right role, they can access protected endpoints.

---

## 🚀 Technologies

- ASP.NET Core Web API
- Entity Framework Core (EF Core)
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- Swagger for API testing
- Postman for manual testing

---

## 🔧 What You Can Do

- **Register a User**  
  You can create a new user by sending a request to the register endpoint.  
  After registration, the user gets the **Admin** role by default.

- **Login and Get Token**  
  A registered user can log in and receive a **JWT token**.  
  This token includes user information like email, username, full name, and role.

- **Access Products (Admin Only)**  
  Only users with the **Admin** role can access the `/api/products` endpoint.  
  This endpoint returns a list of products from the database.

---

## 📌 API Endpoints

| Method | URL                    | Auth Needed | Description             |
|--------|------------------------|-------------|-------------------------|
| POST   | `/api/users/register`  | ❌ No       | Create a new user       |
| POST   | `/api/users/login`     | ❌ No       | Login and get a token   |
| GET    | `/api/products`        | ✅ Yes      | Only for Admin users    |

---

## 💡 What I Learned

- How to create and validate **JWT tokens**
- How to use **Identity** with custom `AppUser` and `AppRole`
- How to use **role-based authorization**
- How to test APIs using **Swagger** and **Postman**
- How to connect an API to **SQL Server** with **EF Core**

<img width="1358" height="1122" alt="Screenshot 2025-08-08 170852" src="https://github.com/user-attachments/assets/79601e28-1aec-4a7f-a927-c6d0ad82fc08" />
<img width="1334" height="1093" alt="Screenshot 2025-08-08 170921" src="https://github.com/user-attachments/assets/11aaa2da-5d67-4736-a360-14202505c600" />
<img width="1282" height="991" alt="Screenshot 2025-08-08 172109" src="https://github.com/user-attachments/assets/3651b88f-5065-4ec7-8e15-0985e6210e1d" />

