from socket import *
from Printer import *
from Settings import *
from Encryptor import *
from Crypto.Cipher import AES

settings = Settings()
encryptor = Encryptor(AES.new(b'01234567890123456789012345678901'))
sock = socket()
sock.connect(('127.0.0.1', int(settings.getSetting('my_port'))))
print 'connected'
sock.send(encryptor.encrypt('you are a faggot'))
print 'sent'
print sock.recv(1028)