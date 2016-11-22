# It is try to create a game like Ragnarok Online.

### What's done!
1. Elementary class Location.
2. Abstract class Person.
3. Elementary class Mob.
4. Determine action and skill structure.
5. Start write connection class.

### What need add else
1. General issues:
  - Need add Loot class;
  - Need add Chat.
2. Location class:
  - Relief;
  - Pathfinding.
3. Person class:
  - Element of atack and defense;
  - Player actions handler;
  - Level and experience system;
  - Skill tree;
  - Equipment;
  - List of passive skills;
  - Event to change stats;
  - Inventory;
  - Riding;
4. Network class:
  - Development of communication protocols;
  - Reception and transmission of data to multiple streams; **Done!** (may be)
  - Server load testing.
5. Loot class:
  - Add derived class of Equip;
  - Some options of loot;
6. Database:
  - Table with accaunts; **Done!**
  - Table with mobs; **Done!**
  - Table with players; **Done!**
  - List of skills (only C#).
  - Table with loots;
  
  That's all!!
  New changes will be marked **Done!** in list.
  
  Wow, wow, wow... Someone else... The most important thing - **NEED ADD GAME CLIENT**
  
  P.S.
 Protocol must transmit:
  - registration; **Done!**
  - login; **Done!**
  - create person; **Done!**
  - delete person; **Done!**
  - select person (go to the world);
  - using actions and skills;
  - stats upgrade;
  - skills upgrade;
  - change equip;
  - using loot.
