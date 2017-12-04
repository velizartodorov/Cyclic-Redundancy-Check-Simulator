using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace cyclicCode

{
    public partial class Form1 : Form
    {
        public string inputValue;
        public string binaryBitStream = null;
        public string compareTextBoxValue = null;
        public int kLocal = 0;
        public int counter = 0;
        public int errorsCount = 0;
        public string inputValueBuffer = "test";
        public string correctString = null;

        // timer variables

        int minutes;
        int seconds;
        int hours;
        int micro;

        public Form1()
        {
            
            // escape button

            this.KeyPreview = true;

            // variables
   
            InitializeComponent();

            seconds = minutes = hours = micro = 0;

            secondsLabel.Text = appendZero(seconds);
            minutesLabel.Text = appendZero(minutes);
            hoursLabel.Text = appendZero(hours);

            var skinManager = MaterialSkinManager.Instance;
        //    skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Blue100, TextShade.WHITE);

        }

        public string XOR(string bits, string generatingPolynom)
        {
            if (bits.Length >= generatingPolynom.Length)
            {
                // put values from string to array

                char[] inputValueArray = bits.ToCharArray();
                char[] generatingPolynomArray = generatingPolynom.ToCharArray();

                string remainder = null;

                // XOR inputValue & generatingPolynom

                for (int i = 0; i < generatingPolynom.Length; i++)
                {
                    remainder += (inputValueArray[i] ^ generatingPolynomArray[i]).ToString();
                }

                // adding remainder to result

                bits = (remainder + bits.Substring(generatingPolynom.Length));

                // trim leading zeros

               bool isAllZeros = bits.All(c => c == '0');

                if (!isAllZeros)
                {
                    bits = bits.TrimStart(new Char[] { '0' });
                }
                else if (isAllZeros)
                {
                    bits = "0";
                }       
            }
            return bits;
        }


        public string calculateRemainderPoylnomial(string remainderPolynomial)
        {
            remainderPolynomial = "x" + (inputValueBuffer.Length - 1 - kLocal).ToString();

            if (remainderPolynomial == "x1")
            return "x";
            if (remainderPolynomial == "x0")
            return "1";
            else
            return remainderPolynomial;
        }


        public void polynomialRepresentation(string inputValue, string generatingPolynom, string binaryBitStream)
        {
            string generatingPolynomView = null;
            string generatingPolynomInput = generatingPolynom;
            generatingPolynomListView.Visible = false;
            polynomStepenLabel.Visible = false;
            chooseButton.Location = new Point(3, 102);
            chooseButton.Size = new Size(210, 36);
            bitStreamLabelIndicator.Visible = true;
            bitStreamLabelIndicator.Text = "X = " + inputValue;
            chooseButton.Text = "Enter/Въведи!";


            // multiplying input by "k"

            binaryBitStream = inputValue;

            if (doubleErrorRadioButton.Checked)
            {
                for (int i = 0; i < k; i++)
                {
                    inputValue += "0";
                }
            }

            if (tripleErrorRadioButton.Checked)
            {
                for (int i = 0; i < (k + 1); i++)
                {
                    inputValue += "0";
                }


                // calculate generating polynom ot triple error

                char[] generatingPolynomArrayLeadingZero = (generatingPolynom.PadLeft(generatingPolynom.Length + 1, '0')).ToCharArray();
                char[] generatingPolynomArrayPadedZero = (generatingPolynom.PadRight(generatingPolynom.Length + 1, '0')).ToCharArray();

                generatingPolynom = null;

                for (int i = 0; i < generatingPolynomArrayPadedZero.Length; i++)
                {
                    generatingPolynom += (generatingPolynomArrayLeadingZero[i] ^ generatingPolynomArrayPadedZero[i]).ToString();
                }
            }


            generatingPolynomView = generatingPolynom;

                for (int i = generatingPolynom.Length; i < inputValue.Length; i++)
                {
                    generatingPolynomView += "0";
                }

            if (richTextBox1.Visible == false)
            {
                groupBox2.Visible = true;

                label6.Text = null;
                label6.Font = new Font("Arial", 15, FontStyle.Bold);
                label6.ForeColor = Color.White;

                label3.Font = new Font("Arial", 12, FontStyle.Bold);
                label3.ForeColor = Color.White;
                label3.Text = "Enter polynomial G(x) starting from the highest power. Example: x3+x2+x+1";
                label3.Text += "\nВъведете полинома G(x) като започнете от най-високата степен.Пример: x3 + x2 + x + 1";
                enterGXTextBox.Visible = true;


                label6.Text += "xᵏ.G(x) = x";

                if (doubleErrorRadioButton.Checked)
                {
                    label6.Text += letterToSuperscript(k.ToString());
                }

                if (tripleErrorRadioButton.Checked)
                {
                    label6.Text += letterToSuperscript((k + 1).ToString());
                }

                label6.Text += " . " + "(" + letterToSuperscript(binaryToPolynomial(binaryBitStream));

                // trim whitespaces            

                enterGXTextBox.Text = Regex.Replace(enterGXTextBox.Text, " ", "");

                // compare binary bitstream with textbox

                if (enterGXTextBox.Text == compareTextBoxValue)
                {
                    enterGXTextBox.Visible = false;
                }
                else if ((enterGXTextBox.Text != compareTextBoxValue) && !(string.IsNullOrWhiteSpace(enterGXTextBox.Text)))
                {
                    printMessage("G(x)", false);
                }
                }
                // compare multiplicated polynomial with textbox;

                if (enterGXTextBox.Text == compareTextBoxValue && richTextBox1.Visible == false)
                {
                    label3.Text = "Multiplicate xᵏ.G(x) starting from the highest power. Example: x3 + x2 + x + 1 \nУмножете полиномите xᵏ.G(x) като започнете от най-високата степен. Пример: x3 + x2 + x + 1";
                    label6.Text += ")" + " = \n = " + letterToSuperscript(binaryToPolynomial(inputValue));
                    enterMultipGXKtextBox.Visible = true;

                    // trim whitespaces            

                    enterMultipGXKtextBox.Text = Regex.Replace(enterMultipGXKtextBox.Text, " ", "");

                    if (enterMultipGXKtextBox.Text == compareTextBoxValue)
                    {
                        enterMultipGXKtextBox.Visible = false;
                    }
                    else if ((enterMultipGXKtextBox.Text != compareTextBoxValue) && !(string.IsNullOrWhiteSpace(enterMultipGXKtextBox.Text)))
                    {
                        printMessage("xᵏ.G(x)", false);
                    }
                }

                if (tripleErrorRadioButton.Checked && ((enterMultipGXKtextBox.Text == compareTextBoxValue)) && richTextBox1.Visible == false)
                {
                    groupBox3.Visible = true;
                    label3.ForeColor = Color.FromArgb(255, 60, 60);
                    label3.Text = "Calculate (1+x).P1(x) starting from the highest power. Example: x3 + x2 + x + 1 \nИзчислете (1+x).P1(x) като започнете от най-високата степен. Пример: x3 + x2 + x + 1";
                    label2.Text = "P(x) = (1+x).P₁(x) = (1+x).(" + letterToSuperscript(binaryToPolynomial(generatingPolynomInput));
                    label2.Text += "); \nP(x) = " + letterToSuperscript(binaryToPolynomial(generatingPolynom));

                    // trim whitespaces            

                    generatingPolynomialtextBox.Visible = true;

                    generatingPolynomialtextBox.Text = Regex.Replace(generatingPolynomialtextBox.Text, " ", "");

                    if (generatingPolynomialtextBox.Text == compareTextBoxValue)
                    {
                        generatingPolynomialtextBox.Visible = false;
                    }
                    else if ((generatingPolynomialtextBox.Text != compareTextBoxValue) && !(string.IsNullOrWhiteSpace(generatingPolynomialtextBox.Text)))
                    {
                        printMessage("(1+x).P₁(x)", false);
                    }

                }

                if ((generatingPolynomialtextBox.Text == compareTextBoxValue && tripleErrorRadioButton.Checked) || (enterMultipGXKtextBox.Text == compareTextBoxValue && doubleErrorRadioButton.Checked))
                {
                    if (textBox1.Visible == false)
                    {
                        label3.Font = new Font("Roboto", 13, FontStyle.Bold);
                        label3.ForeColor = Color.GreenYellow;
                        label3.Text = "Въведете входната последователност xᵏ.G(x) като полином; Пример: x3 + x2 + x + 1\nEnter binary bitstream  xᵏ.G(x) in polynomial; Example: x3 + x2 + x + 1:";
                        label6.ForeColor = Color.GreenYellow;
                        resultLabel.Visible = true;
                        resultLabel.Text = "Резултат / Result: ";
                        resultLabel.BringToFront();
                        inputValueBuffer = inputValue;
                        label6.Font = new Font("Arial", 13, FontStyle.Bold);
                        if (doubleErrorRadioButton.Checked)
                        label6.Text = "xᵏ.G(x) = x" + letterToSuperscript(k.ToString());
                        if (tripleErrorRadioButton.Checked)
                        label6.Text = "xᵏ.G(x) = x" + letterToSuperscript((k + 1).ToString());
                        label6.Text += " . " + "(" + letterToSuperscript(binaryToPolynomial(binaryBitStream));
                        label6.Text += ") = " + letterToSuperscript(binaryToPolynomial(inputValue)) + "; \n";
                        

                            if (doubleErrorRadioButton.Checked)
                            {
                                groupBox3.Visible = true;
                                kLocal = k;
                                label2.ForeColor = Color.OrangeRed;
                                label2.Text = "P(x) = " + letterToSuperscript(binaryToPolynomial(generatingPolynom));
                                label6.Text += "X = " + binaryBitStream + "; l₀ = 2; d ≥ l₀ + 1; d = 3; m =  " + binaryBitStream.Length + "; ";
                                label6.Text += "2ᵏ – k ≥ m + 1 = " + (int)( binaryBitStream.Length + 1) + "; " + "k = " + k + ";";
                            } 


                            if (tripleErrorRadioButton.Checked)
                            {
                                label6.Font = new Font("Arial", 12, FontStyle.Bold);
                                kLocal = k + 1;
                                label6.Text += "X = " + binaryBitStream + "; l₀ = 3; d ≥ l₀ + 1; d ≥ 4; m =  " + binaryBitStream.Length + "; ";
                                label6.Text += "n = " + (int)(binaryBitStream.Length + k) + "; ";
                                label6.Text += "2ᵏ – k ≥ m + 1 = " + (int)(binaryBitStream.Length + 1) + "; " + "k = s + 1 = " + k + " + 1 = " + kLocal + ";";
                            }
                    }
                        richTextBox1.Visible = true;
                        textBox1.Visible = true;
                }
            if (richTextBox1.Visible == true)
            {
                string result = null;

                if (inputValueBuffer.Length >= generatingPolynom.Length && richTextBox1.Visible == true)
                {
                    textBox1.Text = Regex.Replace(textBox1.Text, " ", "");

                    if (counter == 0 && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                    {
                        binaryToPolynomial(inputValueBuffer);

                        if (textBox1.Text == compareTextBoxValue && result != "0")
                        {
                            label3.ForeColor = Color.Fuchsia;
                            label3.Font = new Font("Roboto", 13, FontStyle.Bold);
                            label3.Text = "Въведете резултат от умноженито на xᵏ.G(x) и xᵏ; Пример: x3 + x2 + x + 1 \nEnter result from multiplication of xᵏ.G(x) and xᵏ; Example: x3 + x2 + x + 1";
                            richTextBox1.AppendText("\n");
                            richTextBox1.AppendText("\t");
                            richTextBox1.SelectionColor = Color.GreenYellow;
                            richTextBox1.AppendText(letterToSuperscript(binaryToPolynomial(inputValue)));
                            richTextBox1.SelectionColor = Color.White;
                            richTextBox1.AppendText("\t | ");
                            richTextBox1.SelectionColor = Color.FromArgb(255, 60, 60);
                            richTextBox1.AppendText(letterToSuperscript(binaryToPolynomial(generatingPolynom)));
                            richTextBox1.AppendText("\n");
                            textBox1.Text = String.Empty;
                            counter = 1;
                        }

                    }

                    if (counter == 1 && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                    {
                        result = calculateRemainderPoylnomial(inputValueBuffer);

                        if (textBox1.Text == result && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                        {
                            label3.ForeColor = Color.FromArgb(255, 60, 60);
                            label3.Font = new Font("Roboto", 12, FontStyle.Bold);
                            label3.Text = "Въведете резултата от умножението на (xᵏ.G(x) / xᵏ) и пораждащия полином P(x); Пр:x3 + x2 + x + 1\nEnter result of multiplication of (xᵏ.G(x) / xᵏ) and generating polynomial P(x); Еxample: x3 + x2 + x + 1";
                            textBox1.Text = String.Empty;
                            if (result != "1" && result != "x")
                                resultLabel.Text += letterToSuperscript(result) + " + ";
                            if (result == "1")
                                resultLabel.Text += "1";
                            if (result == "x")
                                resultLabel.Text += "x + ";
                            counter = 2;
                            richTextBox1.SelectionColor = Color.FromArgb(255, 60, 60);
                        }

                    }

                    if (counter == 2 && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                    {
                        generatingPolynomView = generatingPolynom;

                        for (int i = generatingPolynom.Length; i < inputValueBuffer.Length; i++)
                        {
                            generatingPolynomView += "0";
                        }

                        binaryToPolynomial(generatingPolynomView);

                        if (textBox1.Text == compareTextBoxValue && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                        {
                            label3.ForeColor = Color.GreenYellow;
                            label3.Font = new Font("Roboto", 12, FontStyle.Bold);
                            label3.Text = "Въведете „сума по модул 2“ ⊕ на (xᵏ.G(x)/xᵏ)*P(x)) и остатъка на (xᵏ.G(x)); Пример: x3 + x2 + x + 1\nEnter “mod 2“ ⊕ of (xᵏ.G(x)/xᵏ)*P(x)) and remainder (xᵏ.G(x)); Example: x3 + x2 + x + 1:";
                            richTextBox1.AppendText("\t");
                            richTextBox1.SelectionColor = Color.FromArgb(255, 60, 60);
                            richTextBox1.AppendText(letterToSuperscript(binaryToPolynomial(generatingPolynomView)));
                            richTextBox1.AppendText("\n");
                            richTextBox1.AppendText("\t");
                            richTextBox1.SelectionColor = Color.White;
                            richTextBox1.AppendText("——————");
                            richTextBox1.SelectionColor = Color.GreenYellow;
                            richTextBox1.AppendText("\n");
                            textBox1.Text = String.Empty;
                            counter = 3;
                        }

                    }

                    if (counter == 3 && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                    {
                        result = XOR(inputValueBuffer, generatingPolynom);

                        if (result != "0")
                        {
                            binaryToPolynomial(result);
                        }

                        if (textBox1.Text == compareTextBoxValue && result != "0")
                        {
                            label3.ForeColor = Color.Fuchsia;
                            label3.Font = new Font("Roboto", 13, FontStyle.Bold);
                            label3.Text = "Въведете резултата от делението на xᵏ.G(x) и xᵏ; Пример: x3 + x2 + x + 1\nEnter result of division xᵏ.G(x) and xᵏ; Example: x3 + x2 + x + 1";
                            richTextBox1.AppendText("\t");
                            richTextBox1.SelectionColor = Color.GreenYellow;
                            richTextBox1.AppendText(letterToSuperscript(binaryToPolynomial(result)));
                            richTextBox1.AppendText("\n");
                            textBox1.Text = String.Empty;
                            counter = 1;
                            inputValueBuffer = result;
                            richTextBox1.SelectionColor = Color.Fuchsia;
                        }
                        else if (textBox1.Text == result && result == "0")
                        {
                            richTextBox1.SelectionColor = Color.Red;
                            richTextBox1.AppendText("\t\t 0");
                            textBox1.Text = String.Empty;
                            inputValueBuffer = null;
                            counter = 1;

                            for (int i = 0; i < kLocal; i++)
                            {
                                inputValueBuffer += "0";
                            }
                        }
                    }

                    if (textBox1.Text != compareTextBoxValue && !(string.IsNullOrWhiteSpace(textBox1.Text)) && result != "0")
                    {
                        printMessage("", false);
                    }
                }

                if (inputValueBuffer.Length < generatingPolynom.Length && richTextBox1.Visible == true || result == "0")
                {
                    if (resultLabel.Text.EndsWith(" "))
                        resultLabel.Text = resultLabel.Text.Substring(0, resultLabel.Text.Length - 3);

                    textBox1.Text = Regex.Replace(textBox1.Text, " ", "");

                    if (counter == 1)
                    {
                        if (result != "0")
                        {
                            richTextBox1.AppendText("\n \n" + "\t" + "R(x) = " + letterToSuperscript(binaryToPolynomial(inputValueBuffer)));
                        }
                        if (result == "0")
                        {
                            richTextBox1.AppendText("\n \n" + "\t" + "R(x) = ");
                        }

                        label3.ForeColor = Color.Red;
                        label3.Font = new Font("Roboto", 13, FontStyle.Bold);
                        label3.Text = "Въведете остатъка R(x) в двоичен вид; Пример: 1111\nEnter remainder R(x) in binary (0 num = k); Example: 1111";
                        counter = 2;
                    }

                    if (counter == 2 && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                    {
                        if (textBox1.Text == inputValueBuffer && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                        {

                            richTextBox1.AppendText(" -> ");
                            richTextBox1.AppendText(inputValueBuffer);
                            richTextBox1.AppendText("\n");
                            richTextBox1.AppendText("\n");
                            richTextBox1.AppendText("\t");
                            richTextBox1.AppendText("F(x) = xᵏ . G(x) + R(x) = ");
                            label3.ForeColor = Color.Aqua;
                            label3.Font = new Font("Roboto", 12, FontStyle.Bold);
                            label3.Text = "Въведете входната последователност и остатъка F(x) = xᵏ.G(x)+R(x) в двоичен вид; Пример: 1111\nEnter binary bitstream and remainder in F(x) = xᵏ.G(x)+R(x) in binary format; Example: 1111";
                            textBox1.Text = String.Empty;
                            counter = 3;
                        }
                        if (textBox1.Text != inputValueBuffer && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                            printMessage("", false);

                    }
                    if (counter == 3 && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                    {
                        if (textBox1.Text == binaryBitStream + inputValueBuffer)
                        {
                            richTextBox1.SelectionColor = Color.Aqua;
                            richTextBox1.AppendText(binaryBitStream + " ");
                            richTextBox1.SelectionColor = Color.Red;
                            richTextBox1.AppendText(inputValueBuffer);
                            textBox1.Text = String.Empty;
                            counter = 4;
                            timer1.Stop();
                            MessageBox.Show("Отговор/Answer: "+ binaryBitStream + " " + inputValueBuffer + " \nБраво! Успешно решихте задачата \nCongratulations! You solved the problem succesefuly! \nБрой грешки / Mistakes count: " + errorsCount + "\nВреме / Time: " + hoursLabel.Text + " : " + minutesLabel.Text + " : " + secondsLabel.Text + " : " + microsecondsLabel.Text);
       
                            // clear screen:

                            richTextBox1.Clear();
                            richTextBox1.Visible = false;
                            groupBox2.Visible = false;
                            groupBox3.Visible = false;
                            errorsCount = 0;
                            errorPanel.Visible = false;
                            inputValue = null;
                            generatingPolynom = null;
                            generatingPolynomView = null;
                            inputValueBuffer = null;
                            compareTextBoxValue = null;
                            binaryBitStream = null;
                            textBox1.Text = null;
                            polynomLengthTextBox.Text = null;
                            dLabelTextBox.Text = null;
                            kLabelTextBox.Text = null;
                            polynomLengthTextBox.Text = null;
                            doubleErrorRadioButton.Checked = false;
                            tripleErrorRadioButton.Checked = false;
                            chooseButton.Visible = false;
                            textBox1.Visible = false;
                            Heading2.Visible = true;
                            doubleErrorRadioButton.Visible = true;
                            tripleErrorRadioButton.Visible = true;
                            calculateButton.Visible = true;
                            enterGXTextBox.Text = null;
                            enterMultipGXKtextBox.Text = null;
                            generatingPolynomialtextBox.Text = null;
                            resultLabel.Visible = false;
                            resultLabel.Text = null;
                            kLabelTextBox.Enabled = true;
                            dLabelTextBox.Enabled = true;
                            polynomLengthTextBox.Enabled = true;
                            counter = 0;
                            materialTabSelector1.Enabled = true;
                            seconds = minutes = hours = micro = 0;
                            panelTimer.Visible = false;
                            bitStreamLabelIndicator.Visible = false;
                        }
                        if (textBox1.Text != inputValue + inputValueBuffer && !(string.IsNullOrWhiteSpace(textBox1.Text)))
                            printMessage("", false);
                    }
                }
            }
        }

        public void printMessage(string inputValue, bool messageType)
        {
            if (messageType)
                MessageBox.Show("Entered value  " + inputValue + " is valid! \nВъведената стойност  " + inputValue + " е валидна!");
            if (!messageType)
            {
                MessageBox.Show("Entered value  " + inputValue + " is not valid! \nВъведената стойност  " + inputValue + " не е валидна!");
                errorPanel.Visible = true;
                errorsCount++;
                errorCountLabel.Text = errorsCount.ToString();
            }
        }
        public void textBoxValidationKeyPress(object sender, KeyPressEventArgs e)
        {
            errorLabel.Visible = false;

            if (!(char.IsNumber(e.KeyChar)) && !(char.IsWhiteSpace(e.KeyChar)) && !(e.KeyChar == (char)8))
            {
                errorLabel.Visible = true;
                errorLabel.Text = null;
                errorLabel.Text = "Enter numbers only\nВъведете числа (0-9)!";
                e.KeyChar = (char)0;
            }
        }

        public void PolynomialValidationKeyPress(object sender, KeyPressEventArgs e)
        {
            errorLabel.Visible = false;

            if (!(char.IsNumber(e.KeyChar)) && !(char.IsWhiteSpace(e.KeyChar)) && !(e.KeyChar == (char)8) && !(e.KeyChar == (char)120) && !(e.KeyChar == (char)32) && !(e.KeyChar == (char)8) && !(e.KeyChar == (char)43))
            {
                errorLabel.Visible = true;
                errorLabel.Text = null;
                errorLabel.Text = "Enter numbers / + / space (0-9) only / Въведете числа (0-9)!  / + / space!";
                e.KeyChar = (char)0;
            }
        }

        public string letterToSuperscript(string polynomialValue)
        {
            // convert numbers to supersripts

            StringBuilder sb = new StringBuilder(polynomialValue);

            for (int i = 0; i < polynomialValue.Length; i++)
            {
                if (polynomialValue[i] == '0')
                {
                    sb[i] = '⁰';
                    polynomialValue = sb.ToString();
                }

                if (polynomialValue[i] == '1' && polynomialValue[i + 1] != '\0')
                {
                    sb[i] = '¹';
                    polynomialValue = sb.ToString();
                }

                if (polynomialValue[i] == '2')
                {
                    sb[i] = '²';
                    polynomialValue = sb.ToString();
                }


                if (polynomialValue[i] == '3')
                {
                    sb[i] = '³';
                    polynomialValue = sb.ToString();
                }


                if (polynomialValue[i] == '4')
                {
                    sb[i] = '⁴';
                    polynomialValue = sb.ToString();
                }

                if (polynomialValue[i] == '5')
                {
                    sb[i] = '⁵';
                    polynomialValue = sb.ToString();
                }

                if (polynomialValue[i] == '6')
                {
                    sb[i] = '⁶';
                    polynomialValue = sb.ToString();
                }

                if (polynomialValue[i] == '7')
                {
                    sb[i] = '⁷';
                    polynomialValue = sb.ToString();
                }

                if (polynomialValue[i] == '8')
                {
                    sb[i] = '⁸';
                    polynomialValue = sb.ToString();
                }

                if (polynomialValue[i] == '9')
                {
                    sb[i] = '⁹';
                    polynomialValue = sb.ToString();
                }

            }

            return polynomialValue;
        }

        public string binaryToPolynomial(string inputValue)
        {
            string polynomialValue = null;

            char[] inputValueArray2 = inputValue.ToCharArray();

            for (int i = 0; i < inputValue.Length; i++)
            {
                if (inputValueArray2[i] == '1')

                {
                    polynomialValue += "x" + (inputValue.Length - 1 - i).ToString() + " + ";

                }
            }

            // cut the last "+" sign

                polynomialValue = polynomialValue.Substring(0, polynomialValue.Length - 2);
            
            // x0 - > 1
            // x1 - > x

           char [] inputValueArray = polynomialValue.ToCharArray();

            for (int i = 0; i < polynomialValue.Length; i++)
            {

                // if x0 -> 1

                if (i > (polynomialValue.Length - 8) && inputValueArray[i] == 'x' && inputValueArray[i + 1] == '0')

                {
                    inputValueArray[i] = '1';
                    inputValueArray[i + 1] = '\0';
                }


                // if x1 -> x

                else if (i > (polynomialValue.Length - 12) && inputValueArray[i] == 'x' && inputValueArray[i + 1] == '1' && inputValueArray[i + 2] == ' ')
                {
                    inputValueArray[i + 1] = ' ';
                }

            }

            compareTextBoxValue = Regex.Replace(new string(inputValueArray), " ", "");

            if (compareTextBoxValue.ToLower().Contains('\0'))
            {
                compareTextBoxValue = compareTextBoxValue.Remove(compareTextBoxValue.Length - 1);
            }
            
            return polynomialValue = new string(inputValueArray);
        }


        public void addToListView()
        {
            generatingPolynomListView.Items.Clear();

            ListViewItem item1 = new ListViewItem("1");
            item1.SubItems.Add("x+1");
            item1.SubItems.Add("1");

            ListViewItem item2 = new ListViewItem("2");
            item2.SubItems.Add("x²+x+1");
            item2.SubItems.Add("3");

            ListViewItem item3 = new ListViewItem("3");
            item3.SubItems.Add("x³+x+1");
            item3.SubItems.Add("7");

            ListViewItem item31 = new ListViewItem("");
            item31.SubItems.Add("x³+x²+1");
            item31.SubItems.Add("");

            ListViewItem item4 = new ListViewItem("4");
            item4.SubItems.Add("x⁴+x+1");
            item4.SubItems.Add("15");

            ListViewItem item41 = new ListViewItem("");
            item41.SubItems.Add("x⁴+x³+1");
            item41.SubItems.Add("");

            ListViewItem item5 = new ListViewItem("5");
            item5.SubItems.Add("x⁵+x²+1");
            item5.SubItems.Add("31");

            ListViewItem item51 = new ListViewItem("");
            item51.SubItems.Add("x⁵+x³+1");
            item51.SubItems.Add("");

            ListViewItem item6 = new ListViewItem("6");
            item6.SubItems.Add("x⁶+x+1");
            item6.SubItems.Add("63");

            ListViewItem item61 = new ListViewItem("");
            item61.SubItems.Add("x⁶+x⁵+1");
            item61.SubItems.Add("");

            ListViewItem item7 = new ListViewItem("7");
            item7.SubItems.Add("x⁷+x+1");
            item7.SubItems.Add("127");

            ListViewItem item71 = new ListViewItem("");
            item71.SubItems.Add("x⁷+x³+1");
            item71.SubItems.Add("");

            ListViewItem item8 = new ListViewItem("8");
            item8.SubItems.Add("x⁸+x⁴+x³+x²+1");
            item8.SubItems.Add("255");

        
            ListViewItem item81 = new ListViewItem("");
            item81.SubItems.Add("x⁸+x⁶+x⁵+x⁴+1");
            item81.SubItems.Add("");

            ListViewItem item9 = new ListViewItem("9");
            item9.SubItems.Add("x⁹+x⁵+1");
            item9.SubItems.Add("511");

            ListViewItem item91 = new ListViewItem("");
            item91.SubItems.Add("x⁹+x⁴+1");
            item91.SubItems.Add("");

            ListViewItem item10 = new ListViewItem("10");
            item10.SubItems.Add("x¹⁰+x⁷+1");
            item10.SubItems.Add("1023");

            ListViewItem item101 = new ListViewItem("");
            item101.SubItems.Add("x¹⁰+x³+1");
            item101.SubItems.Add("");
 
               generatingPolynomListView.Items.AddRange(new ListViewItem[] 
                { item1, item2, item3, item31, item4, item41,item5, item51,
                  item6, item61, item7, item71, item8, item81, item9, item91, item10, item101 });
        }

        private bool buttonChooseWasClicked = false;
 //       private bool buttonCalculateWasClicked = false;
       
        int k = 0;

        private void button1_Click_1(object sender, EventArgs e)
        {
            EnterInfoGroupBox.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            richTextBox1.Visible = false;
            materialTabSelector1.Enabled = false;
  //          buttonCalculateWasClicked = true;
            panelTimer.Visible = true;

            timer1.Start();


            // loading values in listview:

            addToListView();

            // picking a random binary bit stream from a list:

            // string[] hardCoredInputValues = { "10101011011", "11111111111", "10101010101", "10111010101" };

                string[] hardCoredInputValues = { "1110" };
                inputValue = hardCoredInputValues[new Random().Next(0, hardCoredInputValues.Length)];
                binaryBitStreamLabel.Text = null;
                binaryBitStreamLabel.Text = "X = " + inputValue.ToString();

                errorLabel.Visible = false;
                EnterInfoGroupBox.Visible = true;
                chooseButton.Visible = true;

                 //calculating k

                 k = 0;

                 for (k = 0; k < 10; k++)
                {
                    if ((Math.Pow(2, k) - k) >= (int)(inputValue.Length + 1))
                    {
                        break;
                    }
                }

                Heading2.Visible = false;
                doubleErrorRadioButton.Visible = false;
                tripleErrorRadioButton.Visible = false;
                calculateButton.Visible = false;

                lzeroLabel.Text = null;

                if (doubleErrorRadioButton.Checked)
                {
                    lzeroLabel.Text = "Двойна грешка; l₀ = 2;";
                }
                else if (tripleErrorRadioButton.Checked)
                {
                    lzeroLabel.Text = "Тройна грешка; l₀ = 3;";
                }

        }


        private void chooseButton_Click_1(object sender, EventArgs e)
        {
            chooseButton.Location = new Point(441, 346);
            chooseButton.Text = "Enter/Въведи!";


            buttonChooseWasClicked = true;
            chooseButton.Visible = true;
            chooseButton.BringToFront();

            if ((polynomLengthTextBox.Text == inputValue.Length.ToString()) && buttonChooseWasClicked)
            {
                polynomLengthTextBox.Enabled = false;
                panel2.Visible = true;

                if (doubleErrorRadioButton.Checked && dLabelTextBox.Text == "3")
                {
                    dLabelTextBox.Enabled = false;
                    panel3.Visible = true;

                    if (kLabelTextBox.Text == k.ToString() && kLabelTextBox.Enabled == true)
                    {
                        kLabelTextBox.Enabled = false;
                        printMessage("k", true);
                    }
                    else if (!(string.IsNullOrWhiteSpace(kLabelTextBox.Text)) && (kLabelTextBox.Text != k.ToString()))
                    {
                        printMessage("k", false);
                    }
                }
                else if (tripleErrorRadioButton.Checked && dLabelTextBox.Text == "4")
                {
                    dLabelTextBox.Enabled = false;
                    panel3.Visible = true;


                    if (kLabelTextBox.Text == k.ToString() && kLabelTextBox.Enabled == true)
                    {
                        kLabelTextBox.Enabled = false;
                        printMessage("k", true);

                    }
                    else if (!(string.IsNullOrWhiteSpace(kLabelTextBox.Text)) && (kLabelTextBox.Text != k.ToString()))
                    {
                        printMessage("k", false);
                    }
                }
                else if (!(string.IsNullOrWhiteSpace(dLabelTextBox.Text)) && (dLabelTextBox.Text != "3" || dLabelTextBox.Text != "4"))
                {
                    printMessage("d", false);
                }
            }
            else if(((polynomLengthTextBox.Text != inputValue.Length.ToString()) && buttonChooseWasClicked))
            {
                printMessage("m", false);
            }


            if(!(string.IsNullOrWhiteSpace(kLabelTextBox.Text)) && kLabelTextBox.Enabled == false)
            {
                    EnterInfoGroupBox.Visible = false;
                    chooseButton.Text = "Choose/Избери!";
                    chooseButton.Visible = true;
                    chooseButton.BringToFront();
                    if (groupBox2.Visible == false)
                    {
                        chooseButton.Location = new Point(720, 65);
                    }
                    polynomStepenLabel.Visible = true;
                    generatingPolynomListView.Visible = true;
                
                // ask user for chosing generating polynomial

                int neededK = 0;

                if (generatingPolynomListView.Items[0].Selected && buttonChooseWasClicked)
                {
                    neededK = 1;

                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "11", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[1].Selected && buttonChooseWasClicked)
                {
                    neededK = 2;

                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "111", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[2].Selected && buttonChooseWasClicked)
                {
                    neededK = 3;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "1011", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[3].Selected && buttonChooseWasClicked)
                {
                    neededK = 3;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "1101", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[4].Selected && buttonChooseWasClicked)
                {
                    neededK = 4;
                    if(k == neededK)
                    {
                        polynomialRepresentation(inputValue, "10011", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[5].Selected && buttonChooseWasClicked)
                {
                    neededK = 4;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "11001", binaryBitStream);
                    }

                }
                if (generatingPolynomListView.Items[6].Selected && buttonChooseWasClicked)
                {
                    neededK = 5;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "100101", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[7].Selected && buttonChooseWasClicked)
                {
                    neededK = 5;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "101001", binaryBitStream);
                    }

                }
                if (generatingPolynomListView.Items[8].Selected && buttonChooseWasClicked)
                {
                    neededK = 6;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "1000011", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[9].Selected && buttonChooseWasClicked)
                {
                    neededK = 6;
                    if (k == neededK)
                    {     
                         polynomialRepresentation(inputValue, "1100001", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[10].Selected && buttonChooseWasClicked)
                {
                    neededK = 7;
                        if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "10000011", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[11].Selected && buttonChooseWasClicked)
                {
                    neededK = 7;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "10001001", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[12].Selected && buttonChooseWasClicked)
                {
                    neededK = 8;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "100011101", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[13].Selected && buttonChooseWasClicked)
                {
                    neededK = 8;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "101110001", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[14].Selected && buttonChooseWasClicked)
                {
                    neededK = 9;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "1000100001", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[15].Selected && buttonChooseWasClicked)
                {
                    neededK = 9;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "1000010001", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[16].Selected && buttonChooseWasClicked)
                {
                    neededK = 10;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "10010000001", binaryBitStream);
                    }
                }
                if (generatingPolynomListView.Items[17].Selected && buttonChooseWasClicked)
                {
                    neededK = 10;
                    if (k == neededK)
                    {
                        polynomialRepresentation(inputValue, "10000001001", binaryBitStream);
                    }
                }

                if (generatingPolynomListView.SelectedItems.Count > 0 && neededK != k)
                {
                    printMessage("P₁(x)", false);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            } 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult closingDialog = MessageBox.Show("Are you sure you want to exit \nСигурни ли сте, че искате да излезете?", "Exit/Изход", MessageBoxButtons.YesNo);
            if(closingDialog == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (closingDialog == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void textBox_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            errorLabel.Visible = false;

            if (!(e.KeyChar == (char)8) && !(e.KeyChar == (char)48) && !(e.KeyChar == (char)49))
            {
                errorLabel.Visible = true;
                errorLabel.Text = null;
                errorLabel.Text = "Enter 0 or 1!";
                e.KeyChar = (char)0;
            }
                
        }

        private void polynomLengthTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBoxValidationKeyPress(sender, e);
        }

        private void dLabelTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBoxValidationKeyPress(sender, e);
        }

        private void kLabelTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBoxValidationKeyPress(sender, e);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // scroll it automatically
            richTextBox1.ScrollToCaret();
        }

        private string appendZero(double str)
        {
            if (str <= 9)
                return "0" + str;
            else
                return str.ToString();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            micro++;

            if (micro > 9)
            {
                seconds++;
                micro = 0;
            }
            if (seconds > 59)
            {
                minutes++;
                seconds = 0;
            }
            if (minutes > 59)
            {
                hours++;
                minutes = 0;
            }

            secondsLabel.Text = appendZero(seconds);
            minutesLabel.Text = appendZero(minutes);
            hoursLabel.Text = appendZero(hours);
            microsecondsLabel.Text = appendZero(micro);
        }
        private void enterGXTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
          //  PolynomialValidationKeyPress(sender, e);
        }

        private void enterMultipGXKtextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
         //   PolynomialValidationKeyPress(sender, e);
        }

        private void generatingPolynomialtextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
          //  PolynomialValidationKeyPress(sender, e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
          //  PolynomialValidationKeyPress(sender, e);
        }
    }
}