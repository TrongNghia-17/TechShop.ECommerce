namespace TechShop.ECommerce.Identity.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Table
        builder.ToTable("RefreshTokens");

        // Key
        builder.HasKey(refreshToken => refreshToken.Id);

        // Properties
        builder.Property(refreshToken => refreshToken.TokenHash)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(refreshToken => refreshToken.UserId)
            .IsRequired();

        builder.Property(refreshToken => refreshToken.CreatedAtUtc)
            .IsRequired();

        builder.Property(refreshToken => refreshToken.ExpiresAtUtc)
            .IsRequired();

        builder.Property(refreshToken => refreshToken.RevokedAtUtc);

        builder.Property(refreshToken => refreshToken.ReplacedByTokenHash)
            .HasMaxLength(128);

        // Indexes
        builder.HasIndex(refreshToken => refreshToken.TokenHash)
            .IsUnique();

        builder.HasIndex(refreshToken => refreshToken.UserId);

        // Relationships
        builder.HasOne(refreshToken => refreshToken.User)
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}