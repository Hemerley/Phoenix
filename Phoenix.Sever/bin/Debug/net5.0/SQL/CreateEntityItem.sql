CREATE TABLE EntityItems (
  ID integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  EntityID integer,
  ItemId integer,
  DropChance  real,
  ItemAmount   integer,
  /* Foreign keys */
  CONSTRAINT FK_EntityId
    FOREIGN KEY (EntityID)
    REFERENCES Entities(ID), 
  CONSTRAINT FK_ItemId
    FOREIGN KEY (ItemId)
    REFERENCES Items(ID)
);