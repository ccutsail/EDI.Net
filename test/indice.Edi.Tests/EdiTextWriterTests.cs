﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace indice.Edi.Tests
{
    public class EdiTextWriterTests
    {
        [Fact, Trait(Traits.Tag, "Writer")]
        public void WriterWrites_ServiceStringAdvice_Test() {
            var expected = "UNA:+.? '";
            var output = new StringBuilder();
            var grammar = EdiGrammar.NewEdiFact();
            using (var writer = new EdiTextWriter(new StringWriter(output), grammar)) {
                writer.WriteServiceStringAdvice();
            }
            Assert.Equal(expected, output.ToString().TrimEnd());
        }


        [Fact, Trait(Traits.Tag, "Writer")]
        public void WriterWritesStructureTest() {
            var grammar = EdiGrammar.NewEdiFact();
            var expected =
@"UNA:+.? '
UNB+UNOC:3+1234567891123:14+7080005059275:14:SPOTMARKED+101012:1104+HBQ001++++1'
UNH+1+QUOTES:D:96A:UN:EDIEL2+S'
";
            var output = new StringBuilder();
            using (var writer = new EdiTextWriter(new StringWriter(output), grammar)) {
                writer.WriteServiceStringAdvice();
                writer.WriteToken(EdiToken.SegmentName, "UNB");         Assert.Equal("UNB", writer.Path);
                writer.WriteToken(EdiToken.String, "UNOC");             Assert.Equal("UNB[0][0]", writer.Path);
                writer.WriteToken(EdiToken.ComponentStart);             Assert.Equal("UNB[0][1]", writer.Path);
                writer.WriteToken(EdiToken.Integer, 3);                 Assert.Equal("UNB[0][1]", writer.Path);
                writer.WriteToken(EdiToken.ElementStart);               Assert.Equal("UNB[1]", writer.Path);
                writer.WriteToken(EdiToken.String, "1234567891123");    Assert.Equal("UNB[1][0]", writer.Path);
                writer.WriteToken(EdiToken.Integer, 14);                Assert.Equal("UNB[1][1]", writer.Path);
                writer.WriteToken(EdiToken.ElementStart);               Assert.Equal("UNB[2]", writer.Path);
                writer.WriteValue(7080005059275);                       Assert.Equal("UNB[2][0]", writer.Path);
                writer.WriteValue(14);                                  Assert.Equal("UNB[2][1]", writer.Path);
                writer.WriteValue("SPOTMARKED");                        Assert.Equal("UNB[2][2]", writer.Path);
                writer.WriteToken(EdiToken.ElementStart);               Assert.Equal("UNB[3]", writer.Path);
                writer.WriteValue(new DateTime(2012, 10, 10, 11, 04, 0), "ddMMyy"); Assert.Equal("UNB[3][0]", writer.Path);
                writer.WriteValue(new DateTime(2012, 10, 10, 11, 04, 0), "HHmm");   Assert.Equal("UNB[3][1]", writer.Path);
                writer.WriteToken(EdiToken.ElementStart);               Assert.Equal("UNB[4]", writer.Path);
                writer.WriteValue("HBQ001");                            Assert.Equal("UNB[4][0]", writer.Path);
                writer.WriteToken(EdiToken.ElementStart);               Assert.Equal("UNB[5]", writer.Path);
                writer.WriteValue((string)null);                        Assert.Equal("UNB[5][0]", writer.Path);
                writer.WriteToken(EdiToken.ElementStart);               Assert.Equal("UNB[6]", writer.Path);
                writer.WriteValue((string)null);                        Assert.Equal("UNB[6][0]", writer.Path);
                writer.WriteToken(EdiToken.ElementStart);               Assert.Equal("UNB[7]", writer.Path);
                writer.WriteToken(EdiToken.ElementStart);               Assert.Equal("UNB[8]", writer.Path);
                writer.WriteValue(1);                                   Assert.Equal("UNB[8][0]", writer.Path);

                writer.WriteToken(EdiToken.SegmentName, "UNH");         Assert.Equal("UNH", writer.Path);
                writer.WriteValue(1);                                   Assert.Equal("UNH[0][0]", writer.Path);
                writer.WriteToken(EdiToken.ElementStart);               Assert.Equal("UNH[1]", writer.Path);
                writer.WriteValue("QUOTES");                            Assert.Equal("UNH[1][0]", writer.Path);
                writer.WriteValue('D');                                 Assert.Equal("UNH[1][1]", writer.Path);
                writer.WriteValue("96A");                               Assert.Equal("UNH[1][2]", writer.Path);
                writer.WriteValue("UN");                                Assert.Equal("UNH[1][3]", writer.Path);
                writer.WriteValue("EDIEL2");                            Assert.Equal("UNH[1][4]", writer.Path);
                writer.WriteToken(EdiToken.ElementStart);               Assert.Equal("UNH[2]", writer.Path);
                writer.WriteValue("S");                                 Assert.Equal("UNH[2][0]", writer.Path);
            }
            Assert.Equal(expected, output.ToString());
        }
    }
}
