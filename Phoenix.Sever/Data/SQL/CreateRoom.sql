CREATE TABLE Rooms (
  ID            integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name          text,
  Area          text,
  Status        integer,
  Type          integer,
  Description   text,
  Exits         text,
  Tile          integer,
  North         integer,
  South         integer,
  West          integer,
  East          integer,
  Up            integer,
  Down          integer,
  KeyModeNorth  integer,
  KeyModeSouth  integer,
  KeyModeWest   integer,
  KeyModeEast   integer,
  KeyModeUp     integer,
  KeyModeDown   integer,
  KeyNameNorth  text,
  KeyNameSouth  text,
  KeyNameWest   text,
  KeyNameEast   text,
  KeyNameUp     text,
  KeyNameDown   text,
  KeyTypeNorth  integer,
  KeyTypeSouth  integer,
  KeyTypeWest   integer,
  KeyTypeEast   integer,
  KeyTypeUp     integer,
  KeyTypeDown   integer,
  KeyPassNorth  text,
  KeyPassSouth  text,
  KeyPassWest   text,
  KeyPassEast   text,
  KeyPassUp     text,
  KeyPassDown   text,
  KeyFailNorth  text,
  KeyFailSouth  text,
  KeyFailWest   text,
  KeyFailEast   text,
  KeyFailUp     text,
  KeyFailDown   text,
  Script        text,
  /* Foreign keys */
  CONSTRAINT FK_KeyModeDown
    FOREIGN KEY (KeyModeDown)
    REFERENCES RoomKeyModes(ID), 
  CONSTRAINT FK_KeyModeUp
    FOREIGN KEY (KeyModeUp)
    REFERENCES RoomKeyModes(ID), 
  CONSTRAINT FK_KeyModeEast
    FOREIGN KEY (KeyModeEast)
    REFERENCES RoomKeyModes(ID), 
  CONSTRAINT FK_KeyModeWest
    FOREIGN KEY (KeyModeWest)
    REFERENCES RoomKeyModes(ID), 
  CONSTRAINT FK_KeyModeNorth
    FOREIGN KEY (KeyModeNorth)
    REFERENCES RoomKeyModes(ID), 
  CONSTRAINT FK_KeyModeSouth
    FOREIGN KEY (KeyModeSouth)
    REFERENCES RoomKeyModes(ID), 
  CONSTRAINT FK_Type
    FOREIGN KEY (Type)
    REFERENCES RoomTypes(ID), 
  CONSTRAINT FK_Status
    FOREIGN KEY (Status)
    REFERENCES RoomStatuses(ID),
  CONSTRAINT FK_Tile
    FOREIGN KEY (Tile)
    REFERENCES RoomTiles(ID),  
  CONSTRAINT FK_KeyTypeNorth
    FOREIGN KEY (KeyTypeNorth)
    REFERENCES RoomKeyTypes(ID)
  CONSTRAINT FK_KeyTypeSouth
    FOREIGN KEY (KeyTypeSouth)
    REFERENCES RoomKeyTypes(ID)
  CONSTRAINT FK_KeyTypeWest
    FOREIGN KEY (KeyTypeWest)
    REFERENCES RoomKeyTypes(ID)
  CONSTRAINT FK_KeyTypeEast
    FOREIGN KEY (KeyTypeEast)
    REFERENCES RoomKeyTypes(ID)
  CONSTRAINT FK_KeyTypeUp
    FOREIGN KEY (KeyTypeUp)
    REFERENCES RoomKeyTypes(ID)
  CONSTRAINT FK_KeyTypeDown
    FOREIGN KEY (KeyTypeDown)
    REFERENCES RoomKeyTypes(ID)
);