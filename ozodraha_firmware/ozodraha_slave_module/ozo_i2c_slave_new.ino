#include <Wire.h>

int red1 = 0;
int green1 = 1;
int blue1 = 2;
int red2 = 3;
int green2 = 4;
int blue2 = 5;
int red3 = 6;
int green3 = 7;
int blue3 = 10;
int red4 = 11;
int green4 = 12;
int blue4 = 13;

int data;

void setup() {
Wire.begin(83);
Wire.onReceive(lightLED);
}

void loop() {
delay(100);
}
void lightLED(int press) {
    data = Wire.read();
    Serial.write(data);
    switch(data) {
        case 49:
           digitalWrite(red1,HIGH);
           digitalWrite(green1,LOW);
           digitalWrite(blue1,LOW);
            break;
        case 50:
           digitalWrite(red1,LOW);
           digitalWrite(green1,HIGH);
           digitalWrite(blue1,LOW);
            break;
        case 51:
           digitalWrite(red1,LOW);
           digitalWrite(green1,LOW);
           digitalWrite(blue1,HIGH);
            break;
        case 52:
           digitalWrite(red1,LOW);
           digitalWrite(green1,LOW);
           digitalWrite(blue1,LOW);
        case 53:
           digitalWrite(red2,HIGH);
           digitalWrite(green2,LOW);
           digitalWrite(blue2,LOW);
            break;
        case 54:
           digitalWrite(red2,LOW);
           digitalWrite(green2,HIGH);
           digitalWrite(blue2,LOW);
            break;
        case 55:
           digitalWrite(red2,LOW);
           digitalWrite(green2,LOW);
           digitalWrite(blue2,HIGH);
            break;
        case 56:
           digitalWrite(red2,LOW);
           digitalWrite(green2,LOW);
           digitalWrite(blue2,LOW);
        case 57:
           digitalWrite(red3,HIGH);
           digitalWrite(green3,LOW);
           digitalWrite(blue3,LOW);
            break;
        case 58:
           digitalWrite(red3,LOW);
           digitalWrite(green3,HIGH);
           digitalWrite(blue3,LOW);
            break;
        case 59:
           digitalWrite(red3,LOW);
           digitalWrite(green3,LOW);
           digitalWrite(blue3,HIGH);
            break;
        case 60:
           digitalWrite(red3,LOW);
           digitalWrite(green3,LOW);
           digitalWrite(blue3,LOW);
        case 61:
           digitalWrite(red4,HIGH);
           digitalWrite(green4,LOW);
           digitalWrite(blue4,LOW);
            break;
        case 62:
           digitalWrite(red4,LOW);
           digitalWrite(green4,HIGH);
           digitalWrite(blue4,LOW);
            break;
        case 63:
           digitalWrite(red4,LOW);
           digitalWrite(green4,LOW);
           digitalWrite(blue4,HIGH);
            break;
        case 64:
           digitalWrite(red4,LOW);
           digitalWrite(green4,LOW);
           digitalWrite(blue4,LOW);
    }
}
