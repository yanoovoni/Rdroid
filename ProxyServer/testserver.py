from socket import *
from Printer import *
from Settings import *
import base64

settings = Settings()
printer = Printer()
sock = socket()
sock.bind(('0.0.0.0', int(settings.getSetting('main_server_port'))))
sock.listen(69)
proxy, addr = sock.accept()
print 'connected'
message = proxy.recv(1028)
printer.printMessage('yo mama', base64.b64decode(message))
message = proxy.recv(1028)
printer.printMessage('yo mama', base64.b64decode(message))
proxy.send(base64.b64encode('1:Rdroid SERVER\nLOGIN\nresult:success\n'))
print 'sent'
message = proxy.recv(1028)
printer.printMessage('yo mama', base64.b64decode(message))