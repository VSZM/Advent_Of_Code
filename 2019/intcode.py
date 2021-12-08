from typing import List
from abc import ABC, abstractmethod
from enum import IntEnum


class OperandMode(IntEnum):
    POSITION_MODE = 0
    VALUE_MODE = 1
    RELATIVE_MODE = 2

class ProgramPause(Exception):
    pass


class ThreadState(object):

    def __init__(self, memory: List[int], ip: int, input_stream: List[int], output: List[int], name = None, relative_base: int = 0):
        self.memory = memory
        self.ip = ip
        self.input_stream = input_stream
        self.output = output
        self.name = name
        self.relative_base = relative_base

class InstructionBase(ABC):

    def __init__(self, operand1, operand1_mode = None, operand2 = None, operand2_mode = None, operand3 = None,  operand3_mode = None):
        self.operand1 = operand1
        self.operand2 = operand2
        self.operand3 = operand3
        self.operand1_mode = operand1_mode
        self.operand2_mode = operand2_mode
        self.operand3_mode = operand3_mode

    def getPosition(self, state: ThreadState, operand: int, operand_mode: OperandMode) -> int:
        if operand_mode == OperandMode.POSITION_MODE:
            pos = state.memory[operand]
        elif operand_mode == OperandMode.VALUE_MODE:
            pos = operand
        elif operand_mode == OperandMode.RELATIVE_MODE:
            pos = state.relative_base + state.memory[operand]
        else:
            raise ValueError(f'Invalid operand mode of |{operand_mode}|')

        if pos >= len(state.memory):
            missing = pos - len(state.memory) + 1
            state.memory.extend([0] * missing)
        return pos

    @abstractmethod
    def operate(self, state: ThreadState) -> ThreadState:
        pass

class UnaryInstruction(InstructionBase):

    def __init__(self, operand1, operand1_mode = None):
        super().__init__(operand1, operand1_mode)

class BinaryInstruction(InstructionBase):

    def __init__(self, operand1, operand1_mode = None, operand2 = None, operand2_mode = None):
        super().__init__(operand1, operand1_mode, operand2, operand2_mode)


class TernaryInstruction(InstructionBase):

    def __init__(self, operator, operand1, operand1_mode = None, operand2 = None, operand2_mode = None, operand3 = None,  operand3_mode = None):
        super().__init__(operand1, operand1_mode, operand2, operand2_mode, operand3, operand3_mode)
        self.operator = operator

    def operate(self, state: ThreadState) -> ThreadState:
        memory = state.memory
        a = memory[self.getPosition(state, self.operand1, self.operand1_mode)]
        b = memory[self.getPosition(state, self.operand2, self.operand2_mode)]
        value = self.operator(a, b)
        memory[self.getPosition(state, self.operand3, self.operand3_mode)] = value
        
        state.ip += 4
        return state

class ReadInput(UnaryInstruction):

    
    def __init__(self, operand1, operand1_mode):
        super().__init__(operand1, operand1_mode)

    def operate(self, state: ThreadState) -> ThreadState:
        if len(state.input_stream) == 0:
            #print(f'{state.name} reading failed, halting. ip = {state.ip}')
            raise ProgramPause('Empty input stream, waiting')
        #print(f'{state.name} reading {state.input_stream[0]} to {state.memory[self.operand1]}. ip = {state.ip}')
        state.memory[self.getPosition(state, self.operand1, self.operand1_mode)] = state.input_stream[0]
        state.ip += 2
        state.input_stream = state.input_stream[1:]
        return state
    
    def __str__(self):
        d = vars(self).copy()
        return f'ReadInput |{d}|\n'

    __repr__ = __str__

class PrintOutput(UnaryInstruction):

    
    def __init__(self, operand1, operand1_mode):
        super().__init__(operand1, operand1_mode)

    def operate(self, state: ThreadState) -> ThreadState:
        memory = state.memory
        state.output.append(memory[self.getPosition(state, self.operand1, self.operand1_mode)])

        state.ip += 2
        return state
    
    def __str__(self):
        d = vars(self).copy()
        return f'PrintOutput |{d}|\n'

    __repr__ = __str__

class Add(TernaryInstruction):

    def __init__(self, operand1, operand1_mode = None, operand2 = None, operand2_mode = None, operand3 = None,  operand3_mode = None):
        operator = lambda x, y: x + y
        super().__init__(operator, operand1, operand1_mode, operand2, operand2_mode, operand3, operand3_mode)

    def __str__(self):
        d = vars(self).copy()
        del d['operator']
        return f'Add |{d}|\n'

    __repr__ = __str__

class Multiply(TernaryInstruction):

    def __init__(self, operand1, operand1_mode = None, operand2 = None, operand2_mode = None, operand3 = None,  operand3_mode = None):
        operator = lambda x, y: x * y
        super().__init__(operator, operand1, operand1_mode, operand2, operand2_mode, operand3, operand3_mode)


    def __str__(self):
        d = vars(self).copy()
        del d['operator']
        return f'Multiply |{d}|\n'

    __repr__ = __str__


class JumpNonZero(BinaryInstruction):

    def __init__(self, operand1, operand1_mode = None, operand2 = None, operand2_mode = None):
        super().__init__(operand1, operand1_mode, operand2, operand2_mode)

    def operate(self, state: ThreadState) -> ThreadState:
        memory = state.memory
        expr = memory[self.getPosition(state, self.operand1, self.operand1_mode)]
        pos = memory[self.getPosition(state, self.operand2, self.operand2_mode)]
        if expr:
            state.ip = pos
        else:
            state.ip += 3

        return state

    def __str__(self):
        d = vars(self).copy()
        return f'JumpNonZero |{d}|\n'

    __repr__ = __str__

class JumpZero(BinaryInstruction):

    def __init__(self, operand1, operand1_mode = None, operand2 = None, operand2_mode = None):
        super().__init__(operand1, operand1_mode, operand2, operand2_mode)

    def operate(self, state: ThreadState) -> ThreadState:
        memory = state.memory
        expr = memory[self.getPosition(state, self.operand1, self.operand1_mode)]
        pos = memory[self.getPosition(state, self.operand2, self.operand2_mode)]
        if not expr:
            state.ip = pos
        else:
            state.ip += 3

        return state

    def __str__(self):
        d = vars(self).copy()
        return f'JumpZero |{d}|\n'

    __repr__ = __str__
    
class LessThan(TernaryInstruction):

    def __init__(self, operand1, operand1_mode = None, operand2 = None, operand2_mode = None, operand3 = None,  operand3_mode = None):
        operator = lambda x, y: int(x < y)
        super().__init__(operator, operand1, operand1_mode, operand2, operand2_mode, operand3, operand3_mode)

    def __str__(self):
        d = vars(self).copy()
        del d['operator']
        return f'LessThan |{d}|\n'

    __repr__ = __str__

class Equals(TernaryInstruction):

    def __init__(self, operand1, operand1_mode = None, operand2 = None, operand2_mode = None, operand3 = None,  operand3_mode = None):
        operator = lambda x, y: int(x == y)
        super().__init__(operator, operand1, operand1_mode, operand2, operand2_mode, operand3, operand3_mode)

    def __str__(self):
        d = vars(self).copy()
        del d['operator']
        return f'Equals |{d}|\n'

    __repr__ = __str__

class RelativeBaseOffset(UnaryInstruction):

    def __init__(self, operand1, operand1_mode = None):
        super().__init__(operand1, operand1_mode)

    def operate(self, state: ThreadState) -> ThreadState:
        memory = state.memory
        offset = memory[self.getPosition(state, self.operand1, self.operand1_mode)]
        state.relative_base += offset

        state.ip += 2
        return state

    def __str__(self):
        d = vars(self).copy()
        return f'RelativeBaseOffset |{d}|\n'

    __repr__ = __str__


def interpretProgram(state: ThreadState, verbose: bool = False) -> List[int]:
    instructions = []
    try:
        while state.memory[state.ip] != 99:
            memory = state.memory
            ip = state.ip
            opcode = str(memory[ip]).rjust(5, '0')
            mode1 = int(opcode[-3])
            mode2 = int(opcode[-4])
            mode3 = int(opcode[0])
            if opcode[-2:] == '01':
                instruction = Add(ip + 1, mode1, ip + 2, mode2, ip + 3, mode3)
            elif opcode[-2:] == '02':
                instruction = Multiply(ip + 1, mode1, ip + 2, mode2, ip + 3, mode3)
            elif opcode[-2:] == '03':
                instruction = ReadInput(ip + 1, mode1)
            elif opcode[-2:] == '04':
                instruction = PrintOutput(ip + 1, mode1)
            elif opcode[-2:] == '05':
                instruction = JumpNonZero(ip + 1, mode1, ip + 2, mode2)
            elif opcode[-2:] == '06':
                instruction = JumpZero(ip + 1, mode1, ip + 2, mode2)
            elif opcode[-2:] == '07':
                instruction = LessThan(ip + 1, mode1, ip + 2, mode2, ip + 3, mode3)
            elif opcode[-2:] == '08':
                instruction = Equals(ip + 1, mode1, ip + 2, mode2, ip + 3, mode3)
            elif opcode[-2:] == '09':
                instruction = RelativeBaseOffset(ip + 1, mode1)
            else:
                raise ValueError(f'Invalid opcode |{opcode}|')
            
            state = instruction.operate(state)
            instructions.append(instruction)
    except ValueError:
        print(state.output)
        print(instructions)
        raise

    if verbose:
        print(instructions)
    
    return state.output


def readProgram(file) -> List[int]:
    return [int(num) for num in file.readline().split(',')]