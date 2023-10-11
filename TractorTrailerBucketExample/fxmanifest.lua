fx_version "cerulean"
game "gta5"

client_script "Client/*.Client.net.dll"
server_script "Server/*.Server.net.dll"

files {
    -- Dependencies
    "*.dll",
    "Client/FxEvents.Client.dll"
}

-- To be turned off in production!
fxevents_debug_mode 'true'

author "Quadruplex"
version "1.0.0"
description "Tractor & trailer routing bucket example."