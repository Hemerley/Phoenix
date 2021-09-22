if defenderID ~= -1 then
	if entityID == defenderID and isPlayer == true then
		Message.Direct(defenderID, "~rYou can't nuke yourself you!")
        return
	end

    if not isPlayer then
        Message.Direct(entityID, "~rYou can't nuke a NPC!")
        return
    end

    local entityName = Entity.Get.Name(entityID, isPlayer)
	local defenderName = Entity.Get.Name(defenderID, defenderIsPlayer)
	local roomID = Entity.Get.Room(defenderID, defenderIsPlayer)
	
	
    essage.Direct(defenderID, "~w" .. Tools.FirstToUpper(entityName) .. "~c waves " .. Entity.Get.HisHer(entityID, isPlayer) .. " hands and you suddenly burst into a million pieces.")
    Message.Direct(defenderID, "~w" .. "You roll over and die.")
	Message.Direct(entityID, "~cYou wave your hand and " .. defenderName .. " explodes into a million pieces!") 
	Message.Room(roomID, "~w" .. Tools.FirstToUpper(entityName) .. "~c waves " .. Entity.Get.HisHer(entityID, isPlayer) .. " hands and " .. defenderName .. " explodes into a million pieces!")
    Entity.Kill(defenderID, defenderIsPlayer, entityID, isPlayer)
end