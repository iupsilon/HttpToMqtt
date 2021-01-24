# HttpToMqtt
Bridge between http and mqtt.

Receives http call and forwards it to mqtt.
This project is useful for interacting with mqtt via http.

In my case, wireless relé (Shelly) I/O url actions can be configured with an http url.
For example, for BUTTON LONG PRESSED URL event I configured:
http://myserver:80/Mqtt/publish?deviceName=bedroom-light&action=button_long_pressed&payload=pressed

You can see the generated url from swagger testing page.

Everytime I perform a button long press an http call starts and an MQTT message is published

MQTT argument is computed in this way: {deviceName}/action






