CREATE TABLE Items (
  ID             integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  Name           text,
  Image          integer,
  Type           integer,
  Slot           integer,
  Value          integer,
  Rarity         integer,
  Weight         integer,
  Damage         integer,
  Strength       integer,
  Agility        integer,
  Intellect      integer,
  Stamina        integer,
  Crit           real,
  Haste          real,
  Mastery        real,
  Versatility    real,
  PhilosophyReq  integer,
  StrengthReq    integer,
  AgilityReq     integer,
  IntellectReq   integer,
  StaminaReq     integer,
  AlignmentReq   integer,
  CritReq        integer,
  Script         text,
  /* Foreign keys */
  CONSTRAINT FK_Rarity
    FOREIGN KEY (Rarity)
    REFERENCES Rarity(ID), 
  CONSTRAINT FK_Slot
    FOREIGN KEY (Slot)
    REFERENCES ItemSlots(ID), 
  CONSTRAINT FK_Type
    FOREIGN KEY (Type)
    REFERENCES ItemTypes(ID), 
  CONSTRAINT FK_PhilosophyReq
    FOREIGN KEY (PhilosophyReq)
    REFERENCES Philosophies(ID)
);