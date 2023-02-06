package hr.fer.oprpp1.gui.calc;

import java.awt.Color;
import java.awt.Container;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.Panel;
import java.util.ArrayList;
import java.util.List;
import java.util.function.DoubleBinaryOperator;

import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.SwingConstants;
import javax.swing.SwingUtilities;
import javax.swing.WindowConstants;

import hr.fer.oprpp1.gui.layouts.CalcLayout;
import hr.fer.oprpp1.gui.layouts.ProzorCalc;
import hr.fer.oprpp1.gui.layouts.RCPosition;
import hr.fer.zemris.java.gui.calc.model.CalcModel;
import hr.fer.zemris.java.gui.calc.model.CalcValueListener;
import hr.fer.zemris.java.gui.calc.model.CalculatorInputException;

public class Calculator extends JFrame implements CalcModel{
	
	private boolean editable;
	private boolean negative;
	private String value;
	private double floatValue;
	private String frozenValue;
	private double activeOperand;
	private DoubleBinaryOperator operation;
	private boolean inverse;
	private List<CalcValueListener> list;
	private List<Double> stack;
	
	public Calculator() {
		editable = true;
		negative = false;
		value = "";
		floatValue = 0;
		activeOperand = Double.NaN;
		list = new ArrayList<>();
		inverse = false;
		stack = new ArrayList<>();
		setLocation(20, 50);
		setSize(1000,1000);
		setTitle("JavaCalculator v1.0");
		setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);
		initGUI();
		setSize(750,350);
	}
	
	public static void main(String[] args) {
		SwingUtilities.invokeLater(new Runnable() {
			@Override
			public void run() {
				Calculator c = new Calculator();
				c.setVisible(true);
			}
		});
	}

	private void initGUI() {
		Container cp = getContentPane();
		cp.setLayout(new CalcLayout(10));
		JLabel rezultat = new JLabel();
		rezultat.setBorder(BorderFactory.createRaisedBevelBorder());
		rezultat.setBackground(Color.yellow);
		rezultat.setOpaque(true);
		rezultat.setText(value);
		rezultat.setHorizontalAlignment(SwingConstants.RIGHT);
		rezultat.setFont(rezultat.getFont().deriveFont(30f));
		cp.add(rezultat,"1,1");
		NumberButton[] numbers = new NumberButton[10];
		for(int i = 0; i < 10; i++) {
			numbers[i] = new NumberButton(i);
			numbers[i].setFont(numbers[i].getFont().deriveFont(30f));
			if(i == 0) cp.add(numbers[i], new RCPosition(5,3));
			else if(i > 0 && i < 4) cp.add(numbers[i], new RCPosition(4,2+i));
			else if(i > 3 && i < 7) cp.add(numbers[i], new RCPosition(3,-1+i));
			else cp.add(numbers[i], new RCPosition(2,-4+i));
			int num = Integer.valueOf(i);
			numbers[i].addActionListener((e)->{
				insertDigit(num);
			});;
		}
		JCheckBox inv = new JCheckBox();
		inv.setBackground(Color.cyan);
		inv.setOpaque(true);
		inv.setText("Inv");
		cp.add(inv,"5,7");
		//1/x
		OperationButton krozx = new OperationButton("1/x");
		cp.add(krozx,"2,1");
		krozx.addActionListener((e)->{
			setValue((double)1 / floatValue);
		});
		// log
		OperationButton log = new OperationButton("log");
		log.addActionListener((e)->{
			if(inverse) {
				setValue(Math.pow(10, floatValue));
			}else {
				setValue(Math.log10(floatValue));
			}
		});
		cp.add(log,"3,1");
		// ln
		OperationButton ln = new OperationButton("ln");
		ln.addActionListener((e)->{
			if(inverse) {
				setValue(Math.pow(Math.E, floatValue));
			}else {
				setValue(Math.log(floatValue));
			}
		});
		cp.add(ln,"4,1");
		// x^n
		OperationButton xn = new OperationButton("x^n");
		xn.addActionListener((e)->{
			if(inverse) {
				DoubleBinaryOperator dbo = getPendingBinaryOperation();
				if( dbo != null) {
					setValue(dbo.applyAsDouble(activeOperand, floatValue));
				}
				setActiveOperand(floatValue);
				setPendingBinaryOperation(new DoubleBinaryOperator() {
					@Override
					public double applyAsDouble(double left, double right) {
						double rez = Math.pow(left, (double)1/right);
						if(rez < 0) negative = true;
						else negative = false;
						return rez;
					}
				});
				editable = true;
			}else {
				DoubleBinaryOperator dbo = getPendingBinaryOperation();
				if( dbo != null) {
					setValue(dbo.applyAsDouble(activeOperand, floatValue));
				}
				setActiveOperand(floatValue);
				setPendingBinaryOperation(new DoubleBinaryOperator() {
					@Override
					public double applyAsDouble(double left, double right) {
						double rez = Math.pow(left, right);
						if(rez < 0) negative = true;
						else negative = false;
						return rez;
					}
				});
				editable = true;
			}
		});
		cp.add(xn,"5,1");
		//sin
		OperationButton sin = new OperationButton("sin");
		sin.addActionListener((e)->{
			if(inverse) {
				setValue(Math.asin(floatValue));
			}else {
				setValue(Math.sin(floatValue));
			}
		});
		cp.add(sin,"2,2");
		// cos
		OperationButton cos = new OperationButton("cos");
		cos.addActionListener((e)->{
			if(inverse) {
				setValue(Math.acos(floatValue));
			}else {
				setValue(Math.cos(floatValue));
			}
		});
		cp.add(cos,"3,2");
		// tan
		OperationButton tan = new OperationButton("tan");
		tan.addActionListener((e)->{
			if(inverse) {
				setValue(Math.atan(floatValue));
			}else {
				setValue(Math.tan(floatValue));
			}
		});
		cp.add(tan,"4,2");
		// ctg
		OperationButton ctg = new OperationButton("ctg");
		ctg.addActionListener((e)->{
			if(!inverse) {
				setValue((double)1/Math.tan(floatValue));
			}else {
				setValue((double)1/Math.atan(floatValue));
			}
		});
		cp.add(ctg,"5,2");
		// =
		OperationButton eq = new OperationButton("=");
		eq.addActionListener((e)->{
			DoubleBinaryOperator dbo = getPendingBinaryOperation();
			if(dbo!=null) {
			setValue(dbo.applyAsDouble(activeOperand, floatValue));
			clearActiveOperand();
			setPendingBinaryOperation(null);
			}
		});
		cp.add(eq,"1,6");
		//div
		OperationButton div = new OperationButton("/");
		div.addActionListener((e)->{
			DoubleBinaryOperator dbo = getPendingBinaryOperation();
			if( dbo != null) {
				setValue(dbo.applyAsDouble(activeOperand, floatValue));
			}
			setActiveOperand(floatValue);
			setPendingBinaryOperation(new DoubleBinaryOperator() {
				@Override
				public double applyAsDouble(double left, double right) {
					if(right == 0) return Double.NaN;
					if(left/right < 0)negative = true;
					else negative = false;
					System.out.println(left+" "+right);
					return left/right;
				}
			});
			editable = true;
			
		});
		cp.add(div,"2,6");
		// mul
		OperationButton mul = new OperationButton("*");
		mul.addActionListener((e)->{
			DoubleBinaryOperator dbo = getPendingBinaryOperation();
			if( dbo != null) {
				setValue(dbo.applyAsDouble(activeOperand, floatValue));
			}
			setActiveOperand(floatValue);
			setPendingBinaryOperation(new DoubleBinaryOperator() {
				@Override
				public double applyAsDouble(double left, double right) {
					if(left*right < 0)negative = true;
					else negative = false;
					return left*right;
				}
			});
			editable = true;
			
		});
		cp.add(mul,"3,6");
		// min
		OperationButton min = new OperationButton("-");
		min.addActionListener((e)->{
			DoubleBinaryOperator dbo = getPendingBinaryOperation();
			if( dbo != null) {
				setValue(dbo.applyAsDouble(activeOperand, floatValue));
			}
			setActiveOperand(floatValue);
			setPendingBinaryOperation(new DoubleBinaryOperator() {
				@Override
				public double applyAsDouble(double left, double right) {
					double rez = left - right;
					if(rez < 0) negative = true;
					else negative = false;
					return left-right;
				}
			});
			editable = true;
			
		});
		cp.add(min,"4,6");
		// plu
		OperationButton plu = new OperationButton("+");
		plu.addActionListener((e)->{
			DoubleBinaryOperator dbo = getPendingBinaryOperation();
			if( dbo != null) {
				setValue(dbo.applyAsDouble(activeOperand, floatValue));
			}
			setActiveOperand(floatValue);
			setPendingBinaryOperation(new DoubleBinaryOperator() {
				@Override
				public double applyAsDouble(double left, double right) {
					double rez = left + right;
					if(rez < 0) negative = true;
					else negative = false;
					return rez;
				}
			});
			editable = true;
			
		});
		cp.add(plu,"5,6");
		//clr
		OperationButton clr = new OperationButton("clr");
		clr.addActionListener((e)->{
			clear();
		});
		cp.add(clr,"1,7");
		// res
		OperationButton res = new OperationButton("reset");
		res.addActionListener((e)->{
			clearAll();
		});
		cp.add(res,"2,7");
		// push
		OperationButton push = new OperationButton("push");
		push.addActionListener((e)->{
			push();
		});
		cp.add(push,"3,7");
		//pop
		OperationButton pop = new OperationButton("pop");
		pop.addActionListener((e)->{
			pop();
		});
		cp.add(pop,"4,7");
		// +/-
		OperationButton sign = new OperationButton("+/-");
		sign.addActionListener((e)->{
			swapSign();
		});
		cp.add(sign,"5,4");
		// push
		OperationButton dot = new OperationButton(".");
		dot.addActionListener((e)->{
			insertDecimalPoint();
		});
		cp.add(dot,"5,5");
		addCalcValueListener((e)->{
			rezultat.setText(e.toString());
		});
		inv.addActionListener((a)->{
			inverse = !inverse;
			if(inverse) {
				log.setText("10^x");
				ln.setText("e^x");
				xn.setText("x^(1/n");
				sin.setText("arcsin");
				cos.setText("arccos");
				tan.setText("arctan");
				ctg.setText("arcctg");
			}else {
				log.setText("log");
				ln.setText("ln");
				xn.setText("x^n");
				sin.setText("sin");
				cos.setText("cos");
				tan.setText("tan");
				ctg.setText("ctg");
			}
		});
	}

	@Override
	public void addCalcValueListener(CalcValueListener l) {
		list.add(l);
	}

	@Override
	public void removeCalcValueListener(CalcValueListener l) {
		list.remove(l);
		
	}

	@Override
	public double getValue() {
		return floatValue;
	}

	@Override
	public void setValue(double value) {
		if(Double.valueOf(value).equals(Double.NaN)) {
			this.value = "NaN";
		}else if(Double.valueOf(value).equals(Double.POSITIVE_INFINITY)){
			this.value = "Infinity";
		}else if(Double.valueOf(value).equals(Double.NEGATIVE_INFINITY)){
			this.value = "-Infinity";
		}else{
			this.value = ""+value;
		}
		floatValue = value;
		editable = false;
		negative = value != Double.NaN && value > 0 ? false : true;
		frozenValue = this.value;
		list.get(0).valueChanged(this);
	}

	@Override
	public boolean isEditable() {
		return editable;
	}

	@Override
	public void clear() {
		value = "";
		frozenValue = null;
		editable = true;
		negative = false;
		list.get(0).valueChanged(this);
	}

	@Override
	public void clearAll() {
		value = "";
		frozenValue = null;
		editable = true;
		activeOperand = Double.NaN;
		negative = false;
		operation = null;
		list.get(0).valueChanged(this);
	}

	@Override
	public void swapSign() throws CalculatorInputException {
		if(!editable) throw new CalculatorInputException();
		floatValue*= -1;
		if(value.length() > 0 && value.charAt(0) == '-') {
			value = value.substring(1);
		}else if(value.length() > 0){
			value = "-"+value;
		}
		negative = negative ? false : true; 
		frozenValue = null;
		list.get(0).valueChanged(this);
	}

	@Override
	public void insertDecimalPoint() throws CalculatorInputException {
		if(value.contains(".")) {
			throw new CalculatorInputException();
		}
		if(value.length() == 0 && negative)throw new CalculatorInputException();
		if(value.length() == 0)throw new CalculatorInputException();
		value = value+".";
		list.get(0).valueChanged(this);
	}

	@Override
	public void insertDigit(int digit) throws CalculatorInputException, IllegalArgumentException {
		if(!editable) throw new CalculatorInputException();
		String temp = value;
		Double tempd = floatValue;
		boolean check = floatValue == 0 && value.length() == 1 && digit == 0;
		if(!check) {
			try {
				if(value.equals("0")) value ="";
				else if(value.equals("-0")) value = "-";
				value+=""+digit;
				floatValue = Double.parseDouble(value);
			}catch(Exception e) {
				value = temp;
				throw new CalculatorInputException();
			}
		}
		if(!Double.isFinite(floatValue)) {
			floatValue = tempd;
			value = temp;
			throw new CalculatorInputException();
		}
		list.get(0).valueChanged(this);
	}

	@Override
	public boolean isActiveOperandSet() {
		if(Double.valueOf(activeOperand).isNaN()) return false;
		return true;
	}

	@Override
	public double getActiveOperand() throws IllegalStateException {
		if(Double.isNaN(activeOperand)) throw new IllegalStateException();
		return activeOperand;
	}

	@Override
	public void setActiveOperand(double activeOperand) {
		this.activeOperand = activeOperand;
		value = "";
		floatValue = 0;
	}

	@Override
	public void clearActiveOperand() {
		activeOperand = Double.NaN;
	}

	@Override
	public DoubleBinaryOperator getPendingBinaryOperation() {
		if(operation == null) return null;
		return operation;
	}

	@Override
	public void setPendingBinaryOperation(DoubleBinaryOperator op) {
		operation = op;
	}
	
	@Override
	public String toString() {
		if(frozenValue != null) {
			return frozenValue;
		}else if(value.equals("")) return negative ? "-0" : "0";
		return value;
	}	
	
	public void push() {
		stack.add(floatValue);
	}

	public void pop() {
		if(stack.size() != 0) {
			Double temp = stack.get(stack.size()-1);
			stack.remove(stack.size()-1);
			setValue(temp);
			list.get(0).valueChanged(this);
		}
	}
}
