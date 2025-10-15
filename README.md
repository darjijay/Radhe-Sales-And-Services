# Radhe Sales And Services

Radhe Sales and Services is a responsive ASP.NET Core MVC application designed for electrical retailers that want a lightweight stock management system. The solution ships with in-memory storage, sample data, and dashboards so you can showcase the product catalog, customers, and sales operations quickly.

## Features

- 📊 **Dashboard** with key metrics, low stock alerts, and recent sales insights.
- 📦 **Product management** to create, edit, and delete inventory items with category assignments and reorder thresholds.
- 🗂️ **Category management** for organizing products in logical groups.
- 🙍 **Customer management** to capture buyer contact information.
- 🧾 **Sales capture** workflow that adjusts stock levels and keeps historical sale records.
- 💡 **Responsive layout** built with Bootstrap 5 to look great on desktops, tablets, and phones.

## Getting Started

1. Ensure you have the [.NET 8.0 SDK](https://dotnet.microsoft.com/download) installed.
2. Restore dependencies and run the web project:

   ```bash
   dotnet restore src/RadheSalesAndServices.Web/RadheSalesAndServices.Web.csproj
   dotnet run --project src/RadheSalesAndServices.Web/RadheSalesAndServices.Web.csproj
   ```

3. Open your browser and navigate to `https://localhost:7126` (or the HTTP address shown in the console).

The in-memory repository is pre-populated with sample categories, products, customers, and sales so you can explore the experience immediately. Update `SeedData` in `Data/SeedData.cs` to customise the bootstrap data for your store.
