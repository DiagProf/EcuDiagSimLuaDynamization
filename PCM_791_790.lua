DIDs = {
	["22 35"] = "47 11",
}

PCM = {
	RequestId = 0x791,
	ResponseId = 0x790,
	testi = {
	["22 35"] = "47 11",
	},
	tDIDs = {
		["22 35"] = "47 11",
		["55 35"] = "66 66 77 11",
    },
	Raw = {
		["10 01"] = "50 01 00 64 07 9E",
		["22 F1 10"] = "62 F1 10 54 68 69 73 49 73 41 54 65 73 74 53 74 72 69 6E 67 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
		["22 F1 11"] = "62 F1 11 48 65 6C 6C 6F 57 6F 6C 72 64",
		["10 03"] = "50 03 00 64 07 9E",
		["22 F1 92"] = "62 F1 92 39 30 41",
		["22 F1 99"] = "62 F1 99 20 21 11 11",
		["22 F1 80"] = "62 F1 80 01 56 65 72 2E 30 32 32 34",
		["22 F1 90"] = "62 F1 90 5A 44 46 5A 52 34 39 42 30 30 30 31 32 33 34 35 36",
		["22 F1 87"] = "62 F1 87 36 31 39 31 31 32 36 36 31 30 30",
		["22 F1 98"] = "62 F1 98 12 67",
		["22 F1 8C"] = "62 F1 8C 32 31 32 30 32 36 36 32 31 30 35 31 30 30 30 30 32",
		["19 02 09"] = "59 02 09",
		["22 10 0C"] = "62 10 0C 00 07 81 A7",
		["22 10 0D"] = "62 10 0D 00 07 82 FB",
		["22 10 0E"] = "62 10 0E 00 00 00 00",
		["22 10 0F"] = "62 10 0F 00 00 00 00",
		["22 10 00"] = "62 10 00 01",
		["22 10 03"] = "62 10 03 00 3C",
		["22 10 10"] = "62 10 10 12 C0",
		["22 10 11"] = "62 10 11 05 DC",
		["22 10 12"] = "62 10 12 0E D8",
		["22 10 13"] = "62 10 13 07 6C",
		["22 10 14"] = "62 10 14 12 C0",
		["22 10 15"] = "62 10 15 08 FC",
		["22 10 16"] = "62 10 16 10 CC",
		["22 10 17"] = "62 10 17 08 FC",
		["22 10 18"] = "62 10 18 09 60",
		["22 10 19"] = "62 10 19 0B 54",
		["22 10 1A"] = "62 10 1A 00 50",
		["22 10 1B"] = "62 10 1B 00 50",
		["22 10 1C"] = "62 10 1C 00 96",
		["22 10 1D"] = "62 10 1D 00 96",
		["22 10 20"] = "62 10 20 03 E8",
		["22 10 21"] = "62 10 21 01 F4",
		["3E 00"] = "7E 00",
		["14 FF FF FF"] = "54",
		["22 00 10"] = "62 00 10 03 E8",
		["22 00 14"] = "62 00 14 03",
		["22 10 07"] = "62 10 07 01 04",
		["22 10 05"] = "62 10 05 02 58",
		["22 00 17"] = "62 00 17 04 B0",
		["22 10 0A"] = "62 10 0A 05",
		["22 00 0B"] = "62 00 0B 00",
		["22 00 20"] = "62 00 20 46",
		["22 00 01"] = "62 00 01 01",
		["22 00 23"] = "62 00 23 01",
		["22 00 25"] = "62 00 25 3C",
		["22 00 08"] = "62 00 08 00",
		["22 00 18"] = "62 00 18 07 D0",
		["22 00 19"] = "62 00 19 00 32",
		["22 00 24"] = "62 00 24 02",
		["22 00 15"] = "62 00 15 05",
		["22 00 00"] = "62 00 00 02",
		["22 00 11"] = "62 00 11 03 20",
		["22 00 03"] = "62 00 03 00",
		["22 00 02"] = "62 00 02 01",
		["22 10 0B"] = "62 10 0B 03",
		["22 00 21"] = "62 00 21 05",
		["22 10 08"] = "62 10 08 00 8C",
		["22 00 05"] = "62 00 05 00",
		["22 00 07"] = "62 00 07 03 E8",
		["22 00 09"] = "62 00 09 00",
		["22 00 1A"] = "62 00 1A 05 DC",
		["22 00 04"] = "62 00 04 01",
		["22 10 02"] = "62 10 02 00 00",
		["22 00 16"] = "62 00 16 02 BC",
		["22 00 06"] = "62 00 06 00",
		["22 00 0A"] = "62 00 0A 02",
		["22 00 27"] = "62 00 27 01",
		["22 00 22"] = "62 00 22 03",
		["22 00 26"] = "62 00 26 1E",
		["22 10 09"] = "62 10 09 03",
		["10 02"] = "50 02 00 64 07 9E",
		["31 01 FF 00 00 90 00 01 F7 FF"] = "71 01 FF 00 01",
		["31 03 FF 00"] = "71 03 FF 00 01 00",
		["34 00 33 00 90 00 01 68 00"] = "74 20 01 02",
		["36 *"] = function (request) return "76 " .. request:sub(4,5) end,
		["37"] = "77",
		["31 01 02 01 00 90 00 01 F7 FF 3E 34"] = "71 01 02 01 01",
		["31 03 02 01"] = "71 03 02 01 01 00",
		["2E F1 98 12 67"] = "6E F1 98",
		["2E F1 99 20 21 11 11"] = "6E F1 99",
		["11 01"] = "51 01",
		["2E F1 90 5A 44 46 5A 52 34 39 42 30 30 30 31 32 33 34 35 36"] = "6E F1 90",
		["2E 10 0C 00 07 81 A7"] = "6E 10 0C",
		["2E 10 0D 00 07 82 FB"] = "6E 10 0D",
		["2E 10 0E 00 00 00 00"] = "6E 10 0E",
		["2E 10 0F 00 00 00 00"] = "6E 10 0F",
		["2E 10 00 01"] = "6E 10 00",
		["2E 10 03 00 3C"] = "6E 10 03",
		["2E 10 10 12 C0"] = "6E 10 10",
		["2E 10 11 05 DC"] = "6E 10 11",
		["2E 10 12 0E D8"] = "6E 10 12",
		["2E 10 13 07 6C"] = "6E 10 13",
		["2E 10 14 12 C0"] = "6E 10 14",
		["2E 10 15 08 FC"] = "6E 10 15",
		["2E 10 16 10 CC"] = "6E 10 16",
		["2E 10 17 08 FC"] = "6E 10 17",
		["2E 10 18 09 60"] = "6E 10 18",
		["2E 10 19 0B 54"] = "6E 10 19",
		["2E 10 1A 00 50"] = "6E 10 1A",
		["2E 10 1B 00 50"] = "6E 10 1B",
		["2E 10 1C 00 96"] = "6E 10 1C",
		["2E 10 1D 00 96"] = "6E 10 1D",
		["2E 10 20 03 E8"] = "6E 10 20",
		["2E 10 21 01 F4"] = "6E 10 21",
    }
}

-- Function to update DIDs
function updateDidData(ecu, request)
    local key = request:sub(4,8)  -- Extract the DID key from the request
    local value = request:sub(10)  -- Extract the value to be assigned to the DID key
    ecu.DIDs[key] = value  -- Update the value in the DIDs container
    return "6E " .. key  -- Return Response
end