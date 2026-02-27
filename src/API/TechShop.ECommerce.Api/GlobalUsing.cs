global using MediatR;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.OutputCaching;
global using Microsoft.AspNetCore.RateLimiting;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Net.Http.Headers;
global using Scalar.AspNetCore;
global using Serilog;
global using Serilog.Context;
global using System.Threading.RateLimiting;
global using TechShop.ECommerce.Api.Endpoints;
global using TechShop.ECommerce.Api.Extensions;
global using TechShop.ECommerce.Api.Middleware;
global using TechShop.ECommerce.Application;
global using TechShop.ECommerce.Application.Common;
global using TechShop.ECommerce.Application.Common.Offset;
global using TechShop.ECommerce.Application.Exceptions;
global using TechShop.ECommerce.Application.Features.Carts.Commands.AddToCart;
global using TechShop.ECommerce.Application.Features.Carts.Commands.RemoveFromCart;
global using TechShop.ECommerce.Application.Features.Carts.Dtos;
global using TechShop.ECommerce.Application.Features.Carts.Queries.GetCart;
global using TechShop.ECommerce.Application.Features.Identity.Commands.Login;
global using TechShop.ECommerce.Application.Features.Identity.Commands.Register;
global using TechShop.ECommerce.Application.Features.Orders.Commands.PlaceOrder;
global using TechShop.ECommerce.Application.Features.Products.Dtos;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetProductsPaged;
global using TechShop.ECommerce.Domain.Errors;
global using TechShop.ECommerce.Identity;
global using TechShop.ECommerce.Identity.DbContext;
global using TechShop.ECommerce.Identity.Seeding;
global using TechShop.ECommerce.Infrastructure;
global using TechShop.ECommerce.Persistence;
global using TechShop.ECommerce.Persistence.DatabaseContext;
global using TechShop.ECommerce.Persistence.Seeding;







