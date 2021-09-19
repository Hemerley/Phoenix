CREATE TABLE RoomEntities (
  ID        integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  EntityId  integer,
  RoomId    integer,
  /* Foreign keys */
  CONSTRAINT FK_EntityId
    FOREIGN KEY (EntityId)
    REFERENCES Entities(ID), 
  CONSTRAINT FK_RoomId
    FOREIGN KEY (RoomId)
    REFERENCES Rooms(ID)
);