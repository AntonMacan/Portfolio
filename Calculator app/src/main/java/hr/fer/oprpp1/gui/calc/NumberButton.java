package hr.fer.oprpp1.gui.calc;

import java.awt.Color;
import java.awt.Font;

import javax.swing.JButton;

@SuppressWarnings("serial")
public class NumberButton extends JButton {
	private int number;
	
	public NumberButton(int number) {
		this.number = number;
		setName(""+number);
		setText(""+number);
		setBackground(Color.cyan);
	}
	
	public int getValue() {
		return number;
	}
}
