from socket import *
from Printer import *
from Settings import *


settings = Settings()
printer = Printer()
sock = socket()
sock.bind(('0.0.0.0', int(settings.getSetting('main_server_port'))))
sock.listen(69)
proxy, addr = sock.accept()
print 'connected'
message = proxy.recv(1028)
printer.printMessage('yo mama', message)
message = proxy.recv(1028)
printer.printMessage('yo mama', message)
proxy.send('1:you ugly')
print 'sent'
message = proxy.recv(1028)
printer.printMessage('yo mama', message)