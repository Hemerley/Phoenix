CREATE TABLE RoomNPC (
  ID        integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  NPCId  integer,
  RoomId    integer,
  /* Foreign keys */
  CONSTRAINT FK_NPCId
    FOREIGN KEY (NPCId)
    REFERENCES NPC(ID), 
  CONSTRAINT FK_RoomId
    FOREIGN KEY (RoomId)
    REFERENCES Rooms(ID)
);