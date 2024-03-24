YourEcuName = {
	RequestId = 0x7A6,
	ResponseId = 0x7A7,
	DIDs = {
		["22 35"] = "47 11",
	},
	Raw = {
		["10 03"] = "50 03 00 28 00 C8",
		["3E 00"] = "7E 00",
		["22 22 35"] =  function(request) return getDidData(YourEcuName,request) end,
		["2E 22 35 *"] = function(request) return updateDidData(YourEcuName,request ) end,
        -- Additional entries here
    }
}

-- Function to update DIDs
function updateDidData(ecu, request)
    local key = request:sub(4,8)  -- Extract the DID key from the request
    local value = request:sub(10)  -- Extract the value to be assigned to the DID key
    ecu.DIDs[key] = value  -- Update the value in the DIDs container
    return "6E " .. key  -- Return Response
end

-- Function to retrieve a value from DIDs
function getDidData(ecu, request)
    local key = request:sub(4)  -- Extract the key from the request starting from the 4th character
    local value = ecu.DIDs[key]  -- Retrieve the value associated with the key in the DIDs table
    if value then
        return "62 " .. key .. " " .. value  -- Return the Response with the retrieved value
    else
        return "7F 22 11"  -- Return a default error message if the key is not found
    end
end