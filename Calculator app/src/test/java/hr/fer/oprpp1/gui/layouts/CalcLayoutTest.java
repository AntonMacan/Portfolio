package hr.fer.oprpp1.gui.layouts;

import static org.junit.jupiter.api.Assertions.*;

import javax.swing.JLabel;
import javax.swing.JPanel;

import org.junit.jupiter.api.Test;

class CalcLayoutTest {

	@Test
	void prvoOgranicenjeTest() {
		JPanel p = new JPanel(new CalcLayout(3));
		assertThrows(CalcLayoutException.class,()->{p.add(new JLabel("x"), new RCPosition(0,1));});
	}
	
	@Test
	void drugoOgranicenjeTest() {
		JPanel p = new JPanel(new CalcLayout(3));
		assertThrows(CalcLayoutException.class,()->{p.add(new JLabel("x"), new RCPosition(1,3));});
	}
	
	@Test
	void treceOgranicenjeTest() {
		JPanel p = new JPanel(new CalcLayout(3));
		p.add(new JLabel("x"), new RCPosition(1,1));
		assertThrows(CalcLayoutException.class,()->{p.add(new JLabel("x"), new RCPosition(1,1));});
	}

}
