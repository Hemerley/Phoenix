if (entityID ~= -1 and defenderID ~= -1) then
    if (entityID == defenderID and defenderIsPlayer == isPlayer) then
        Message.Direct(entityID, "~rYou cannot attack yourself!");
        return;
    end
    if (defenderIsPlayer == false) then
        if (Entity.Get.TypeID(defenderID, defenderIsPlayer) <= 0) then
            Message.Direct(entityID, "~rYou cannot attack " .. Entity.Get.Name(defenderID, defenderIsPlayer));
            return;
        end
    end
    Attack.Full(entityID, isPlayer, defenderID, defenderIsPlayer, "hands");
end 