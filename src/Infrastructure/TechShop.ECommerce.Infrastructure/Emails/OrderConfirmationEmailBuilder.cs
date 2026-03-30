namespace TechShop.ECommerce.Infrastructure.Emails;

public sealed class OrderConfirmationEmailBuilder
    : IOrderConfirmationEmailBuilder
{
    public EmailMessage Build(Order order)
    {
        var subject = $"Order {order.Id} confirmed";

        var htmlBody = BuildHtmlBody(order);
        var textBody = BuildTextBody(order);

        return new EmailMessage(
            order.CustomerEmail,
            subject,
            htmlBody,
            textBody);
    }

    private static string BuildHtmlBody(Order order)
    {
        var itemsHtml = string.Join("", order.OrderItems.Select(item => $"""
            <tr>
                <td style="padding:12px;border-bottom:1px solid #e5e7eb;">{item.ProductName}</td>
                <td style="padding:12px;border-bottom:1px solid #e5e7eb;text-align:center;">{item.Quantity}</td>
                <td style="padding:12px;border-bottom:1px solid #e5e7eb;text-align:right;">{item.UnitPrice:N0}</td>
                <td style="padding:12px;border-bottom:1px solid #e5e7eb;text-align:right;">{item.UnitPrice * item.Quantity:N0}</td>
            </tr>
            """));

        return $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>Order Confirmation</title>
        </head>
        <body style="margin:0;padding:0;background-color:#f3f4f6;font-family:Arial,Helvetica,sans-serif;color:#111827;">
            <div style="max-width:640px;margin:0 auto;padding:24px;">
                <div style="background-color:#ffffff;border-radius:12px;padding:32px;">
                    <h1 style="margin:0 0 16px;font-size:24px;">Your order is confirmed</h1>

                    <p style="margin:0 0 12px;">Hello,</p>
                    <p style="margin:0 0 20px;">
                        Thank you for your purchase. Your order has been confirmed successfully.
                    </p>

                    <div style="background-color:#f9fafb;border:1px solid #e5e7eb;border-radius:8px;padding:16px;margin-bottom:24px;">
                        <p style="margin:0 0 8px;"><strong>Order ID:</strong> {order.Id}</p>
                        <p style="margin:0 0 8px;"><strong>Order date:</strong> {order.OrderDate:dd/MM/yyyy HH:mm}</p>
                        <p style="margin:0;"><strong>Total amount:</strong> {order.TotalAmount:N0}</p>
                    </div>

                    <h2 style="font-size:18px;margin:0 0 12px;">Order items</h2>

                    <table style="width:100%;border-collapse:collapse;margin-bottom:24px;">
                        <thead>
                            <tr style="background-color:#f9fafb;">
                                <th style="padding:12px;text-align:left;border-bottom:1px solid #e5e7eb;">Product</th>
                                <th style="padding:12px;text-align:center;border-bottom:1px solid #e5e7eb;">Qty</th>
                                <th style="padding:12px;text-align:right;border-bottom:1px solid #e5e7eb;">Price</th>
                                <th style="padding:12px;text-align:right;border-bottom:1px solid #e5e7eb;">Subtotal</th>
                            </tr>
                        </thead>
                        <tbody>
                            {itemsHtml}
                        </tbody>
                    </table>

                    <h2 style="font-size:18px;margin:0 0 12px;">Shipping address</h2>
                    <div style="background-color:#f9fafb;border:1px solid #e5e7eb;border-radius:8px;padding:16px;margin-bottom:24px;">
                        <p style="margin:0 0 6px;">{order.ShippingAddress.Street}</p>
                        <p style="margin:0 0 6px;">{order.ShippingAddress.City}</p>
                        <p style="margin:0 0 6px;">{order.ShippingAddress.PostalCode}</p>
                        <p style="margin:0;">{order.ShippingAddress.Country}</p>
                    </div>

                    <p style="margin:0 0 12px;">
                        We will notify you again when your order is being prepared for delivery.
                    </p>

                    <p style="margin:0;">TechShop Team</p>
                </div>
            </div>
        </body>
        </html>
        """;
    }

    private static string BuildTextBody(Order order)
    {
        var lines = order.OrderItems
            .Select(item => $"- {item.ProductName} x{item.Quantity} - {(item.UnitPrice * item.Quantity):N0}");

        return $"""
        Your order is confirmed.

        Order ID: {order.Id}
        Order date: {order.OrderDate:dd/MM/yyyy HH:mm}
        Total amount: {order.TotalAmount:N0}

        Items:
        {string.Join(Environment.NewLine, lines)}

        Shipping address:
        {order.ShippingAddress.Street}
        {order.ShippingAddress.City}
        {order.ShippingAddress.PostalCode}
        {order.ShippingAddress.Country}

        Thank you for your purchase.
        """;
    }
}