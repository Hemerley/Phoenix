CREATE TABLE Characters (
  ID integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  AccountID integer,
  Name text,
  Image integer,
  Gender text,
  HisHer text,
  HeShe text,
  Experience integer,
  Title text,
  Caste integer,
  Rank integer,
  Philosophy integer,
  Alignment integer,
  Creation integer,
  Strength integer,
  Agility integer,
  Intellect integer,
  Stamina integer,
  Damage integer,
  Health integer,
  Mana integer,
  RoomID integer,
  /* Foreign keys */
  CONSTRAINT FK_AccountID
    FOREIGN KEY (AccountID)
    REFERENCES Accounts(ID), 
  CONSTRAINT FK_RoomID
    FOREIGN KEY (RoomID)
    REFERENCES Rooms(ID)
);