CREATE TABLE "RefreshTokens" (
    "Id" uuid PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "Token" text NOT NULL,
    "ExpiresAt" timestamp with time zone NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "RevokedAt" timestamp with time zone,
    "ReplacedByToken" text,
    CONSTRAINT "FK_Refresh_User" FOREIGN KEY ("UserId")
        REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_RefreshTokens_UserId"
    ON "RefreshTokens" ("UserId");

CREATE UNIQUE INDEX "IX_RefreshTokens_Token"
    ON "RefreshTokens" ("Token");