from socket import *
from Printer import *
from Settings import *


settings = Settings()
printer = Printer()
sock = socket()
sock.bind(('0.0.0.0', int(settings.getSetting('main_server_port'))))
sock.listen(69)
proxy, addr = sock.accept()
message = proxy.recv(1028)
printer.printMessage('yo mama', message)
proxy.send('127.0.0.1:you ugly')
