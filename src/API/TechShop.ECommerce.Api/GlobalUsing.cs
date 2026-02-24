global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Http.HttpResults;
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
global using TechShop.ECommerce.Api.Models;
global using TechShop.ECommerce.Application;
global using TechShop.ECommerce.Application.Common.Cursors;
global using TechShop.ECommerce.Application.Common.Offset;
global using TechShop.ECommerce.Application.Contracts.Identity;
global using TechShop.ECommerce.Application.Exceptions;
global using TechShop.ECommerce.Application.Features.Carts.Commands.AddToCart;
global using TechShop.ECommerce.Application.Features.Carts.Commands.RemoveFromCart;
global using TechShop.ECommerce.Application.Features.Carts.Dtos;
global using TechShop.ECommerce.Application.Features.Carts.Queries.GetCart;
global using TechShop.ECommerce.Application.Features.Orders.Commands.CreateOrder;
global using TechShop.ECommerce.Application.Features.Orders.Dtos;
global using TechShop.ECommerce.Application.Features.Products.Commands.BulkPurge;
global using TechShop.ECommerce.Application.Features.Products.Commands.BulkUpdatePrice;
global using TechShop.ECommerce.Application.Features.Products.Dtos;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetProductsCursor;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetProductsPaged;
global using TechShop.ECommerce.Application.Models.Identity;
global using TechShop.ECommerce.Identity;
global using TechShop.ECommerce.Identity.DbContext;
global using TechShop.ECommerce.Identity.Seeding;
global using TechShop.ECommerce.Infrastructure;
global using TechShop.ECommerce.Persistence;
global using TechShop.ECommerce.Persistence.DatabaseContext;
global using TechShop.ECommerce.Persistence.Seeding;







