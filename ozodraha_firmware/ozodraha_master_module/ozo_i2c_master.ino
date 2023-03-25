#include <Wire.h>
#include <SoftwareSerial.h>
SoftwareSerial bt(0,1);//RX,TX
String flag;
String readString;
byte data[4];
int destination;
void setup() 
{
Wire.begin();
bt.begin(9600);
Serial.begin(9600);
}

void loop() {
  readString = read();
  for(int i = 0;i<sizeof(readString);i++)
  {
    data[i] = readString[i+1];
      if(data[i] > 0)
      {
      Serial.println(data[i]);
      }
  }
  destination = readString[0];
  send(destination,data);
  }
  
  void send(int address, byte data[4])
  {

    for(int i = 0; i <= sizeof(data)+1;i++)
    {                                                
        if(data[i] != 0)
        { 
         Serial.println(data[i]);
         Wire.beginTransmission(address); 
         Wire.write(data[i]);
         Wire.endTransmission();
        }                                              
    }
  }
  String read()
  {
    String readString;
    while(bt.available() > 0)
    { 
    readString = bt.readString();
    readString.trim();
    return readString;
    }
  }

