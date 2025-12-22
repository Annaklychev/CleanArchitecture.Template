-- USERS
CREATE TABLE "AspNetUsers" (
    "Id" uuid NOT NULL PRIMARY KEY,
    "UserName" varchar(256),
    "NormalizedUserName" varchar(256),
    "Email" varchar(256),
    "NormalizedEmail" varchar(256),
    "EmailConfirmed" boolean NOT NULL,
    "IsBlocked" boolean NOT NULL DEFAULT false,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "FirstName" text,
    "LastName" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "AccessFailedCount" integer NOT NULL
);

CREATE UNIQUE INDEX "IX_Users_NormalizedUserName"
    ON "AspNetUsers" ("NormalizedUserName");

CREATE INDEX "IX_Users_NormalizedEmail"
    ON "AspNetUsers" ("NormalizedEmail");

-- ROLES
CREATE TABLE "AspNetRoles" (
    "Id" uuid NOT NULL PRIMARY KEY,
    "Name" varchar(256),
    "NormalizedName" varchar(256),
    "ConcurrencyStamp" text
);

CREATE UNIQUE INDEX "IX_Roles_NormalizedName"
    ON "AspNetRoles" ("NormalizedName");

-- USER ROLES
CREATE TABLE "AspNetUserRoles" (
    "UserId" uuid NOT NULL,
    "RoleId" uuid NOT NULL,
    PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_User" FOREIGN KEY ("UserId")
        REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Role" FOREIGN KEY ("RoleId")
        REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

-- USER CLAIMS
CREATE TABLE "AspNetUserClaims" (
    "Id" serial PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "FK_UserClaims_User" FOREIGN KEY ("UserId")
        REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- ROLE CLAIMS
CREATE TABLE "AspNetRoleClaims" (
    "Id" serial PRIMARY KEY,
    "RoleId" uuid NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "FK_RoleClaims_Role" FOREIGN KEY ("RoleId")
        REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

-- USER LOGINS
CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" varchar(128) NOT NULL,
    "ProviderKey" varchar(128) NOT NULL,
    "ProviderDisplayName" text,
    "UserId" uuid NOT NULL,
    PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_UserLogins_User" FOREIGN KEY ("UserId")
        REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- USER TOKENS
CREATE TABLE "AspNetUserTokens" (
    "UserId" uuid NOT NULL,
    "LoginProvider" varchar(128) NOT NULL,
    "Name" varchar(128) NOT NULL,
    "Value" text,
    PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_UserTokens_User" FOREIGN KEY ("UserId")
        REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);