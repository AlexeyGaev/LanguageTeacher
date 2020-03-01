var previousPlayer = "";
var winnerCellIds = [["", "", ""],["", "", ""]];
var cells = ["","","","","","","","",""];
var canStartGame = false;
var hasComputer = true;
var isFirstComputer = false;
var footer = "";

function StartGame() {
	var elements = document.getElementsByName("gameType");
	hasComputer = elements[0].checked;
	if (!hasComputer && !elements[1].checked)
		return;
	canStartGame = true;
    previousPlayer = "";
	winnerCellIds = [["", "", ""],["", "", ""]];
	cells = ["","","","","","","","",""];
	footer = "";
	DrawPlayerHeader("playerHeader", "", "#000000");
	DrawPlayerHeader("player_1", "", "#000000");
	DrawPlayerHeader("player_2", "", "#000000");
	if (hasComputer){
		isFirstComputer = GetRandomInt(2) == 1;
		if (isFirstComputer) {
			PlaceCore(GetComputerStartCellId(), GetNextPlayer(previousPlayer));
			DrawFooter(footer);
			return;
		}
	}
	DrawCells();
	DrawFooter(footer);
}
function GetNextPlayer(player) {
	return player == "X" ? "0" : "X";
}
function Add(cells, id, player) {
	for (var i = 0; i < 9; i++) 
		if (id == i.toString()) 
			cells[i] = player;
}
function Copy(sourceCells, targetCells) {
	for (var i = 0; i < 9; i++)
		targetCells[i] = sourceCells[i];
}
function InitWinnerCells(cells, winnerCellIds) {
	CheckWinner(cells, winnerCellIds, "0", "1", "2");
	CheckWinner(cells, winnerCellIds, "3", "4", "5");
	CheckWinner(cells, winnerCellIds, "6", "7", "8");
	CheckWinner(cells, winnerCellIds, "0", "3", "6");
	CheckWinner(cells, winnerCellIds, "1", "4", "7");
	CheckWinner(cells, winnerCellIds, "2", "5", "8");
	CheckWinner(cells, winnerCellIds, "0", "4", "8");
	CheckWinner(cells, winnerCellIds, "2", "4", "6");
}
function CheckWinner(cells, winnerCellIds, firstId, secondId, thirdId) {
	var firstText = cells[firstId];
	var secondText = cells[secondId];
	var thirdText = cells[thirdId];
	if(firstText != "" && firstText == secondText && firstText == thirdText){
		if (winnerCellIds[0][0] == "") {
			winnerCellIds[0][0] = firstId;
			winnerCellIds[0][1] = secondId;
			winnerCellIds[0][2] = thirdId;
		}
		else {
			winnerCellIds[1][0] = firstId;
			winnerCellIds[1][1] = secondId;
			winnerCellIds[1][2] = thirdId;
		}
	}
}
function GetWinnerCellId(player, cells) {
	for (var i = 0; i < 9; i++) {
		if (cells[i] == "") {
			cells[i] = player;
			var winnerCellIds = [["", "", ""],["", "", ""]];
			InitWinnerCells(cells, winnerCellIds);
			if (winnerCellIds[0][0] != "")
				return i.toString();
			else 
				cells[i] = "";
		}
	}
	return "";
}
function GetRandomFreeCellId(cells, indexes) {
	var emptyCellIndexes = ["","","",""];
	var emptyCellCurrentIndex = -1;
	for (var i=0; i<indexes.length; i++){
		var currentIndex = indexes[i];
		if (cells[currentIndex] == "") {
			emptyCellCurrentIndex++;
			emptyCellIndexes[emptyCellCurrentIndex] = currentIndex;
		}
	}
	if (emptyCellCurrentIndex == -1)
		return "";
	else 
		return emptyCellIndexes[GetRandomInt(emptyCellCurrentIndex + 1)].toString();
}
function GetRandomInt(max) {
	return Math.floor(Math.random() * max);
}
function GetComputerStartCellId() {
	var cornerCellId = GetRandomFreeCellId(cells, [0,2,6,8]);
	var centerCellId = "4";
	var randomCellId = GetRandomFreeCellId(cells, [1,3,5,7]);
	return cornerCellId; //TODO : randomize other cells by dificult
}
function GetComputerCellId(currentPlayer, computerPlayer) {
	var copyCells = ["","","","","","","","",""];
	Copy(cells, copyCells);
	var winnerCellId = GetWinnerCellId(computerPlayer, copyCells);
	if (winnerCellId != "") {
		footer = "Winner computer";
		return winnerCellId;
	}
	var playerCellId = GetWinnerCellId(currentPlayer, copyCells);
	if (playerCellId != "") {
		footer = "Dont winner player";
		return playerCellId;
	}
	var cornerCellId = GetRandomFreeCellId(cells, [0,2,6,8]);
	if (cornerCellId != "") {
		footer = "Random corner";
		return cornerCellId;
	}
	var centerCellIndex = 4;
	if (cells[centerCellIndex] == "") {
		footer = "Center";
		return centerCellIndex.toString();
	}
	var randomCellId = GetRandomFreeCellId(cells, [1,3,5,7]);
	if (randomCellId != "") {
		footer = "Random line";
		return randomCellId;
	}
	footer = "Empty";
	return "";
}
function IsContinueGame() {
	for (var i = 0; i < 9; i++) 
		if (cells[i] == "")
			return true;
	return false;
}

function Place(box) {
	if (!canStartGame){
		footer = "Can't start game";
		DrawFooter(footer);
		return;
	}
	if (winnerCellIds[0][0] != "") {
		footer = "Has win";
		DrawFooter(footer);
		return;
	}
	if(box.innerText != "") {
		footer = "Cell is busy";
		DrawFooter(footer);
		return;
	}
	if (hasComputer) {
		var currentPlayer = GetNextPlayer(previousPlayer);
		Add(cells, box.id, currentPlayer);	
		InitWinnerCells(cells, winnerCellIds);
		if (winnerCellIds[0][0] != "") {
			DrawWinner(currentPlayer == "X");
			DrawFooter(footer);
			return;
		}
	    var computerPlayer = GetNextPlayer(currentPlayer);
		var computerCellId = GetComputerCellId(currentPlayer, computerPlayer);	
		if (computerCellId != "") {
			PlaceCore(computerCellId, computerPlayer);
			DrawFooter(footer);
			return;
		}
		DrawDontWinner();
		DrawFooter(footer);
		return;
	}
	PlaceCore(box.id, GetNextPlayer(previousPlayer));
	DrawFooter(footer);
}
function PlaceCore(id, player){
	Add(cells, id, player);	
	InitWinnerCells(cells, winnerCellIds);
	previousPlayer = player;
	if (winnerCellIds[0][0] != "") {
		DrawWinner(player == "X");
		return;
	}
	if (IsContinueGame()) {
		DrawCurrentPlayer(player == "X");
		return;
	}
	DrawDontWinner();
}

function DrawCells() {
	for (var i = 0; i < 9; i++) {
		var box = document.getElementById(i.toString());
		box.innerText = cells[i];
		if (box.innerText != "") 
			box.style.color = "#0000FF";
	}
}
function DrawCurrentPlayer(isFirstPlayer){
	DrawPlayerHeader("playerHeader", "Current move", "#00FF00");
	if (isFirstPlayer){
		DrawPlayerHeader("player_1", "Player 1 (X) moved", "#00FF00");
		DrawPlayerHeader("player_2", "Current move - Player 2 (0)", "#000000");
	}
	else {
		DrawPlayerHeader("player_1", "Current move - Player 1 (X)", "#000000");
		DrawPlayerHeader("player_2", "Player 2 (0) moved", "#00FF00");
	}
	DrawCells();
}
function DrawDontWinner() {
	DrawPlayerHeader("playerHeader", "Draw!", "#FF0000");
	DrawPlayerHeader("player_1", "Player 1 (X)", "#000000");
	DrawPlayerHeader("player_2", "Player 2 (0)", "#000000");
	DrawCells();
}
function DrawWinner(isFirstPlayer) {
	DrawPlayerHeader("playerHeader", "Win!", "#FF0000");
	if (isFirstPlayer){
		DrawPlayerHeader("player_1", "Player 1 (X) winner!", "#FF0000");
		DrawPlayerHeader("player_2", "Player 2 (0) lost", "#000000");
	}
	else {
		DrawPlayerHeader("player_1", "Player 1 (X) lost!", "#000000");
		DrawPlayerHeader("player_2", "Player 2 (0) winner", "#FF0000");
	}
	DrawCells();
	DrawWinnerCells();
}
function DrawPlayerHeader(id, text, color) {
	var item = document.getElementById(id);
	item.innerText = text;
	item.style.color = color;
}
function DrawWinnerCells() {
	for (var i = 0; i < 2; i++) {
		for (var j = 0; j < 3; j++) {
			var box = document.getElementById(winnerCellIds[i][j]);
			if (box.innerText != "") 
			box.style.color = "#FF0000";
		}
	}
}
function DrawFooter(value) {
	document.getElementById("footer").innerText = value;
}