YourEcuName = {
	RequestId = 0x7A6,
	ResponseId = 0x7A7,
	Raw = {
		["10 03"] = "50 03 00 28 00 C8",
		["3E 00"] = "7E 00",
		["22 08 15"] = "62 08 15 13", -- will not be changed because there is no writing counterpart to DID 0815
		["22 22 35"] = "62 22 35 47 11",
		["2E 22 35 47 11"] = "6E 22 35",
		["22 F1 99"] = "62 F1 99 20 21 11 11",
		["2E F1 99 20 21 11 11"] = "6E F1 99",
		["22 F1 10"] = "62 F1 10 44 77 11 00", -- will not be changed because DID F110 does not have the same reading and writing length.
		["2E F1 10 44 77"] = "6E F1 10", -- will not be changed because DID F110 does not have the same reading and writing length.
        -- Additional entries here
    }
}
