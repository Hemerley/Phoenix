CREATE TABLE CharacterItems (
  ID           integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  CharacterID  integer,
  ItemId       integer,
  ItemAmount   integer,
  SlotIndex    integer,
  IsEquipped   integer,
  /* Foreign keys */
  CONSTRAINT FK_CharacterId
    FOREIGN KEY (CharacterID)
    REFERENCES Characters(ID), 
  CONSTRAINT FK_ItemId
    FOREIGN KEY (ItemId)
    REFERENCES Items(ID)
);