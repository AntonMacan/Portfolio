package hr.fer.oprpp1.gui.layouts;

import javax.swing.SwingUtilities;

public class Demo {
	public static void main(String[] args) {
		SwingUtilities.invokeLater(new Runnable() {
			@Override
			public void run() {
				ProzorCalc p = new ProzorCalc();
				p.setVisible(true);
			}
		});
	}
}
