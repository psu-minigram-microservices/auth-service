CREATE TABLE IF NOT EXISTS "RefreshSessions" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UserId" UUID NOT NULL,
    "RefreshToken" UUID NOT NULL,
    "Ip" VARCHAR(45) NOT NULL,
    "UserAgent" TEXT NOT NULL,
    "ExpiresIn" TIMESTAMP NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP NOT NULL DEFAULT NOW()
);


ALTER TABLE "RefreshSessions" 
ADD CONSTRAINT "FK_RefreshSessions_Users_UserId" 
FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE;

CREATE INDEX "IX_RefreshSessions_UserId" ON "RefreshSessions" ("UserId");
CREATE INDEX "IX_RefreshSessions_RefreshToken" ON "RefreshSessions" ("RefreshToken");
CREATE INDEX "IX_RefreshSessions_ExpiresIn" ON "RefreshSessions" ("ExpiresIn");