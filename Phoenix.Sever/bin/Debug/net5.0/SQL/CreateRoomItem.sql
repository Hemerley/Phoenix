CREATE TABLE RoomItems (
  ID      integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  RoomId  integer,
  ItemId  integer,
  /* Foreign keys */
  CONSTRAINT FK_RoomId
    FOREIGN KEY (RoomId)
    REFERENCES Rooms(ID), 
  CONSTRAINT FK_ItemId
    FOREIGN KEY (ItemId)
    REFERENCES Items(ID)
);