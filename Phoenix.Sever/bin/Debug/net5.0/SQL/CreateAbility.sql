CREATE TABLE Abilities (
  ID integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name text,
  Philosophy integer,
  Type integer,
  Rank integer,
  Script integer,
  /* Foreign keys */
  CONSTRAINT FK_Rank
    FOREIGN KEY (Rank)
    REFERENCES AbilityRanks(ID), 
  CONSTRAINT FK_Type
    FOREIGN KEY (Type)
    REFERENCES AbilityTypes(ID), 
  CONSTRAINT FK_Philosophy
    FOREIGN KEY (Philosophy)
    REFERENCES Philosophies(ID)
);