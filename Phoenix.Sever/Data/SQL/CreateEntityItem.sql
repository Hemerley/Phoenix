CREATE TABLE NPCItems (
  ID integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  NPCID integer,
  ItemId integer,
  DropChance  real,
  ItemAmount   integer,
  /* Foreign keys */
  CONSTRAINT FK_NPCId
    FOREIGN KEY (NPCID)
    REFERENCES NPC(ID), 
  CONSTRAINT FK_ItemId
    FOREIGN KEY (ItemId)
    REFERENCES Items(ID)
);