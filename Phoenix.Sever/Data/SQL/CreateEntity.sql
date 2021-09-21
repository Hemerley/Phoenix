CREATE TABLE NPC (
  ID           integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  Type         integer,
  Rarity       integer,
  Name         text,
  Image        integer,
  HisHer       text,
  HeShe        text,
  BName        text,
  Level        integer,
  Gold         integer,
  Strength     integer,
  Agility      integer,
  Intellect    integer,
  Stamina      integer,
  Damage       integer,
  Crit         real,
  Haste        real,
  Mastery      real,
  Versatility  real,
  Health       integer,
  Mana         integer,
  Taunt        integer,
  SpawnTime    integer,
  SpawnDelay   integer,
  VanishTime   integer,
  Script       text,
  /* Foreign keys */
  CONSTRAINT FK_Type
    FOREIGN KEY (Type)
    REFERENCES NPCTypes(ID), 
  CONSTRAINT FK_Rarity
    FOREIGN KEY (Rarity)
    REFERENCES Rarity(ID)
);