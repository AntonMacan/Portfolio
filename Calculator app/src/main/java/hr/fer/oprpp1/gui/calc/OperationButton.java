package hr.fer.oprpp1.gui.calc;

import java.awt.Color;
import java.awt.Font;

import javax.swing.JButton;

public class OperationButton extends JButton {
private String operation;
	
	public OperationButton(String operation) {
		this.operation = operation;
		setName(""+operation);
		setText(""+operation);
		setBackground(Color.cyan);
		setFont(this.getFont().deriveFont(2));
	}
	
	public String getOperation() {
		return operation;
	}
}
