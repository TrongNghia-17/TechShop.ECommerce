global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;

global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;

global using TechShop.ECommerce.Application.Common.Constants;
global using TechShop.ECommerce.Application.Contracts.Authentication;
global using TechShop.ECommerce.Application.Contracts.Identity;
global using TechShop.ECommerce.Identity.Context;
global using TechShop.ECommerce.Identity.Entities;