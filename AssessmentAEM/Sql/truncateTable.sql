
ALTER TABLE Wells
DROP CONSTRAINT FK_Wells_Platforms_PlatformId;

TRUNCATE TABLE [Platforms];
TRUNCATE TABLE [Wells];

ALTER TABLE Wells
ADD CONSTRAINT FK_Wells_Platforms_PlatformId
FOREIGN KEY (PlatformId)
REFERENCES Platforms (Id);