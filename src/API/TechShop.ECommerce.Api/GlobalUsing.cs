global using Hangfire;
global using Hangfire.PostgreSql;
global using MediatR;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.EntityFrameworkCore;
global using Scalar.AspNetCore;
global using Serilog;
global using Serilog.Context;
global using System.IO.Compression;
global using System.Threading.RateLimiting;
global using TechShop.ECommerce.Api.Endpoints;
global using TechShop.ECommerce.Api.Extensions;
global using TechShop.ECommerce.Api.Middleware;
global using TechShop.ECommerce.Application;
global using TechShop.ECommerce.Application.Common;
global using TechShop.ECommerce.Application.Common.Paging;
global using TechShop.ECommerce.Application.Exceptions;
global using TechShop.ECommerce.Application.Features.Carts.Commands.AddToCart;
global using TechShop.ECommerce.Application.Features.Carts.Commands.RemoveFromCart;
global using TechShop.ECommerce.Application.Features.Carts.Dtos;
global using TechShop.ECommerce.Application.Features.Carts.Queries.GetCart;
global using TechShop.ECommerce.Application.Features.Identity.Commands.Login;
global using TechShop.ECommerce.Application.Features.Identity.Commands.Register;
global using TechShop.ECommerce.Application.Features.Products.Commands.UpdateProduct;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetProductDetails;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;
global using TechShop.ECommerce.Domain.Errors;
global using TechShop.ECommerce.Identity;
global using TechShop.ECommerce.Infrastructure;
global using TechShop.ECommerce.Persistence;





