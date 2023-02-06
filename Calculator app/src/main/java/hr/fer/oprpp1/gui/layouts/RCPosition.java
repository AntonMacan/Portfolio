package hr.fer.oprpp1.gui.layouts;

public class RCPosition {
	public final int row;
	public final int column;
	public RCPosition(int row, int column) {
		if(row < 1 || row > 5 || column < 1 || column > 7) {
			throw new CalcLayoutException();
		}else if( row == 1 && (column>1 && column < 6)) {
			throw new CalcLayoutException();
		}
		this.row = row;
		this.column = column;
	}
	
	public static RCPosition Parse(String text) {
		String[] pos = text.split(",");
		return new RCPosition(Integer.parseInt(pos[0]),Integer.parseInt(pos[1]));
	}
}
